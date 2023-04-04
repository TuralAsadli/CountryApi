using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Models;
using CountryInfoApi.Utilites.FiIeExtentions;

namespace CountryInfoApi.Service
{
    public class RecomendedPlaceService : IRecomendedPlacesService
    {
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IBaseRepository<City> _cities;
        public readonly IBaseRepository<RecomendedPlace> _db;

        public RecomendedPlaceService(IWebHostEnvironment hostingEnvironment, IBaseRepository<RecomendedPlace> db, IBaseRepository<City> cities)
        {
            _hostingEnvironment = hostingEnvironment;
            _db = db;
            _cities = cities;
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
                    var fileName = item.RenameImg();
                    item.CreateImgFile(Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", fileName));


                    PlaceImg placeimg = new PlaceImg();
                    placeimg.ImgPath = fileName;
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
                var city = _db.Get(guid, c => c.PlaceImgs);
                if (city != null)
                {
                    await _db.Remove(city);
                }
            };
        }

        public IEnumerable<RecomendedPlace> GetAll()
        {
            return _db.GetAll(i => i.PlaceImgs, c => c.City);
        }

        public RecomendedPlace GetById(Guid id)
        {
            return _db.Get(id, c => c.PlaceImgs);
        }

        public async Task UpdateAsync(string id, RecomendedPlaceDto placeDto)
        {
            var existingObject = _db.Get(Guid.Parse(id), c => c.PlaceImgs);

            existingObject.PlaceName = placeDto.PlaceName;
            existingObject.Description = placeDto.Description;
            existingObject.Coordinates = placeDto.Coordinates;
            

            if (placeDto.PlacesImgs != null)
            {
                foreach (var img in existingObject.PlaceImgs)
                {
                    ImgExtention.DeleteImgFile(Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", img.ImgPath));
                }

                List<PlaceImg> placeImgs = new List<PlaceImg>();
                foreach (var img in placeDto.PlacesImgs)
                {
                    if (img.CheckImgFileType())
                    {
                        var fileName = img.RenameImg();
                        img.CreateImgFile(Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", fileName));


                        PlaceImg placeimg = new PlaceImg();
                        placeimg.ImgPath = fileName;
                        placeImgs.Add(placeimg);
                    }
                }
                existingObject.PlaceImgs = placeImgs;
                await _db.Update(existingObject);

            }
        }
    }
}
