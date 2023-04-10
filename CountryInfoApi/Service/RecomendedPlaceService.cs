using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Models;
using CountryInfoApi.Models.Base;
using CountryInfoApi.Utilites.CloudStorage;
using CountryInfoApi.Utilites.FiIeExtentions;
using Microsoft.Extensions.Options;

namespace CountryInfoApi.Service
{
    public class RecomendedPlaceService : IRecomendedPlacesService
    {
        
        public readonly IBaseRepository<City> _cities;
        public readonly IBaseRepository<RecomendedPlace> _db;
        ApiKeys _apiKeys;
        public RecomendedPlaceService(IBaseRepository<RecomendedPlace> db, IBaseRepository<City> cities, IOptions<ApiKeys> apiKeys)
        {
           
            _db = db;
            _cities = cities;
            _apiKeys = apiKeys.Value;
        }

        public async Task CreateAsync(Guid cityId, RecomendedPlaceDto placeDto)
        {
            RecomendedPlace place = new RecomendedPlace()
            {
                PlaceName = placeDto.PlaceName,
                Description = placeDto.Description,
                Coordinates = placeDto.Coordinates,
            };

            List<PlaceImg> placeImgs = new List<PlaceImg>();
            foreach (var item in placeDto.PlacesImgs)
            {
                if (item.CheckImgFileType())
                {
                    CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                    var imgpath = await storage.UploadImageAsync(item, @"Apps/CountryApi");


                    PlaceImg placeimg = new PlaceImg();
                    placeimg.ImgPath = imgpath;
                    placeImgs.Add(placeimg);
                }
            }

            place.PlaceImgs = placeImgs;
            place.City = _cities.Get(cityId, c => c.RecomendedPlaces);
            await _db.Create(place);
        }

        public async Task DeleteAsync(string id)
        {
            if (Guid.TryParse(id, out Guid guid))
            {
                var place = _db.Get(guid, c => c.PlaceImgs);
                if (place != null)
                {
                    CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                    foreach (var img in place.PlaceImgs)
                    {
                        await storage.RemoveImg(img.ImgPath);
                    }
                    await _db.Remove(place);
                }
            };
        }

        public async Task<IEnumerable<RecomendedPlaceGetDto>> GetAll()
        {
            var places = _db.GetAll(i => i.PlaceImgs,c => c.City);
            List<RecomendedPlaceGetDto> placesDto = new();
            foreach (var place in places)
            {
                RecomendedPlaceGetDto placeDto = new RecomendedPlaceGetDto()
                {
                    Id = place.Id,
                    PlaceName = place.PlaceName,
                    Coordinates = place.Coordinates,
                    Description = place.Description,
                    City = new GetCityDto
                    {
                        Id = place.City.Id,
                        Area = place.City.Area,
                        CityName = place.City.CityName,
                        Description = place.City.Description,
                        Population =place.City.Population
                    }

                };
                List<byte[]> imgs = new List<byte[]>();

                foreach (var img in place.PlaceImgs)
                {
                    CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                    var imageBytes = await storage.GetImg(img.ImgPath);
                    imgs.Add(imageBytes);
                }

                placeDto.PlacesImgs = imgs;
                placesDto.Add(placeDto);
            }

            return placesDto;
        }

        public async Task<RecomendedPlaceGetDto> GetById(Guid id)
        {
            var place = _db.Get(id, c => c.PlaceImgs);
            RecomendedPlaceGetDto placeDto = new RecomendedPlaceGetDto()
            {
                Id=place.Id,
                PlaceName = place.PlaceName,
                Coordinates = place.Coordinates,
                Description = place.Description,
                City = new GetCityDto
                {
                    Id = place.City.Id,
                    Area = place.City.Area,
                    CityName = place.City.CityName,
                    Description = place.City.Description,
                    Population = place.City.Population
                }

            };
            List<byte[]> imgs = new List<byte[]>();

            foreach (var img in place.PlaceImgs)
            {
                CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                var imageBytes = await storage.GetImg(img.ImgPath);
                imgs.Add(imageBytes);
            }

            placeDto.PlacesImgs = imgs;

            return placeDto;
        }

        public async Task UpdateAsync(string id, RecomendedPlaceDto placeDto)
        {
            var existingObject = _db.Get(Guid.Parse(id), c => c.PlaceImgs);

            existingObject.PlaceName = placeDto.PlaceName;
            existingObject.Description = placeDto.Description;
            existingObject.Coordinates = placeDto.Coordinates;
            

            if (placeDto.PlacesImgs != null)
            {
                CLoudStorage storage = new CLoudStorage(_apiKeys.Key);
                foreach (var img in existingObject.PlaceImgs)
                {
                    await storage.RemoveImg(img.ImgPath);
                }

                List<PlaceImg> placeImgs = new List<PlaceImg>();
                foreach (var img in placeDto.PlacesImgs)
                {
                    if (img.CheckImgFileType())
                    {
                        var imgpath = await storage.UploadImageAsync(img, @"Apps/CountryApi");


                        PlaceImg placeimg = new PlaceImg();
                        placeimg.ImgPath = imgpath;
                        placeImgs.Add(placeimg);
                    }
                }
                existingObject.PlaceImgs = placeImgs;
                await _db.Update(existingObject);
            }
        }
    }
}
