using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using CountryInfoApi.Utilites.FiIeExtentions;
using Dropbox.Api;
using CountryInfoApi.Models.Base;
using Microsoft.Extensions.Options;
using CountryInfoApi.Utilites.CloudStorage;
using CountryInfoApi.Dtos.RecomendedPlace;

namespace CountryInfoApi.Service
{
    public class CityService : ICityService
    {
        public readonly IBaseRepository<City> _db;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IBaseRepository<RecomendedPlace> _places;
        ApiKeys _apiKeys;

        public CityService(IBaseRepository<City> db, IWebHostEnvironment hostingEnvironment, IOptions<ApiKeys> apiKeys, IBaseRepository<RecomendedPlace> places)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _apiKeys = apiKeys.Value;
            _places = places;
        }

        public async Task CreateAsync(CityDto cityDto)
        {
            City city = new City()
            {
                Area = cityDto.Area,
                CityName = cityDto.CityName,
                Description = cityDto.Description,
                Population = cityDto.Population,
            };

            List<CityImg> cityImgs = new List<CityImg>();
            foreach (var item in cityDto.CityImgs)
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
                var city = _db.Get(guid, c => c.CityImgs);
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
            var cities = _db.GetAll(c => c.CityImgs, p => p.RecomendedPlaces);
            List<GetCityDto> citiesDto = new();
            foreach (var city in cities)
            {
                GetCityDto cityDto = new GetCityDto()
                {
                    Id = city.Id,
                    CityName = city.CityName,
                    Area = city.Area,
                    Description = city.Description,
                    Population = city.Population,
                    

                };
                var places = new List<RecomendedPlaceGetDto>();
                foreach (var place in city.RecomendedPlaces)
                {
                    places.Add(new RecomendedPlaceGetDto()
                    {
                        Id=place.Id,
                        PlaceName = place.PlaceName,
                        Coordinates = place.Coordinates,
                        Description = place.Description,
                    });
                    
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
            var city = _db.Get(id, c => c.CityImgs);
            GetCityDto cityDto = new GetCityDto()
            {
                CityName = city.CityName,
                Area = city.Area,
                Description = city.Description,
                Population = city.Population,

            };

            var places = new List<RecomendedPlaceGetDto>();
            foreach (var place in city.RecomendedPlaces)
            {
                places.Add(new RecomendedPlaceGetDto()
                {
                    PlaceName = place.PlaceName,
                    Coordinates = place.Coordinates,
                    Description = place.Description,
                });

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

            return cityDto;
        }


        public async Task UpdateAsync(string id, CityDto cityDto)
        {
            var existingObject = _db.Get(Guid.Parse(id), c => c.CityImgs);

            existingObject.CityName = cityDto.CityName;
            existingObject.Population = cityDto.Population;
            existingObject.Description = cityDto.Description;
            existingObject.Area = cityDto.Area;

            if (cityDto.CityImgs != null)
            {
                CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                foreach (var img in existingObject.CityImgs)
                {
                    await storage.RemoveImg(img.ImgPath);
                }

                List<CityImg> cityImgs = new List<CityImg>();
                foreach (var img in cityDto.CityImgs)
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
