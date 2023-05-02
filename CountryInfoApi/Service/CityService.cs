using AutoMapper;
using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Models;
using CountryInfoApi.Models.Base;
using CountryInfoApi.Utilites.CloudStorage;
using CountryInfoApi.Utilites.FiIeExtentions;
using Microsoft.Extensions.Options;
using System.Linq;

namespace CountryInfoApi.Service
{
    public class CityService : ICityService
    {
        public readonly IBaseRepository<City> _db;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IBaseRepository<RecomendedPlace> _places;
        private readonly IMapper _mapper;
        ApiKeys _apiKeys;

        public CityService(IBaseRepository<City> db, IWebHostEnvironment hostingEnvironment, IOptions<ApiKeys> apiKeys, IBaseRepository<RecomendedPlace> places, IMapper mapper)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _apiKeys = apiKeys.Value;
            _places = places;
            _mapper = mapper;
        }

        public async Task CreateAsync(CityDto cityDto)
        {
            City city = _mapper.Map<City>(cityDto);

            List<CityImg> cityImgs = new List<CityImg>();
            foreach (var item in cityDto.CityImgsFormFile)
            {
                if (item.CheckImgFileType())
                {
                    CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                    var imgpath = await storage.UploadImageAsync(item, @"Apps/CountryApi");

                    CityImg cityImg = new CityImg();
                    cityImg.ImgPath = imgpath;
                    cityImgs.Add(cityImg);
                }

            }
            city.CityImgs = cityImgs;

            await _db.Create(city);


        }

        public async Task DeleteAsync(string id)
        {
            if (Guid.TryParse(id, out Guid guid))
            {
                var city = await _db.Get(guid, c => c.CityImgs);
                if (city != null)
                {
                    CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                    foreach (var img in city.CityImgs)
                    {
                        await storage.RemoveImg(img.ImgPath);
                    }
                    await _db.Remove(city);
                }
            };
        }

        public async Task<IEnumerable<GetCityDto>> GetAll()
        {
            var cities = await _db.GetAll(c => c.CityImgs, p => p.RecomendedPlaces);
            List<GetCityDto> citiesDto = new();
            foreach (var city in cities)
            {
                GetCityDto cityDto = _mapper.Map<GetCityDto>(city);
                var places = new List<RecomendedPlaceGetDto>();
                foreach (var place in city.RecomendedPlaces)
                {
                    places.Add(_mapper.Map<RecomendedPlaceGetDto>(place));
                }

                cityDto.Places = places;

                List<string> imgs = new List<string>();

                foreach (var img in city.CityImgs)
                {
                    CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                    var imageBytes = await storage.GetImg(img.ImgPath);
                    imgs.Add(Convert.ToBase64String(imageBytes));
                }

                cityDto.cityImgs = imgs;
                citiesDto.Add(cityDto);
            }

            return citiesDto;
        }

        public async Task<GetCityDto> GetById(Guid id)
        {
            var city = await _db.Get(id, c => c.CityImgs, c => c.RecomendedPlaces);
            GetCityDto cityDto = _mapper.Map<GetCityDto>(city);

            var places = (from place in city.RecomendedPlaces
                          select _mapper.Map<RecomendedPlaceGetDto>(place)).ToList();
            cityDto.Places = places;

            List<string> imgs = new List<string>();

            foreach (var img in city.CityImgs)
            {
                CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                var imageBytes = await storage.GetImg(img.ImgPath);
                imgs.Add(Convert.ToBase64String(imageBytes));
            }

            cityDto.cityImgs = imgs;

            return cityDto;
        }


        public async Task UpdateAsync(string id, CityDto cityDto)
        {
            var existingObject = await _db.Get(Guid.Parse(id), c => c.CityImgs);

            existingObject.CityName = cityDto.CityName;
            existingObject.Population = cityDto.Population;
            existingObject.Description = cityDto.Description;
            existingObject.Area = cityDto.Area;

            if (cityDto.CityImgsFormFile != null)
            {
                CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                foreach (var img in existingObject.CityImgs)
                {
                    await storage.RemoveImg(img.ImgPath);
                }

                List<CityImg> cityImgs = new List<CityImg>();
                foreach (var img in cityDto.CityImgsFormFile)
                {
                    if (img.CheckImgFileType())
                    {
                        var imgpath = await storage.UploadImageAsync(img, @"Apps/CountryApi");


                        CityImg cityImg = new CityImg();
                        cityImg.ImgPath = imgpath;
                        cityImgs.Add(cityImg);
                    }
                }
                existingObject.CityImgs = cityImgs;

                await _db.Update(existingObject);

            }
        }
    }
}
