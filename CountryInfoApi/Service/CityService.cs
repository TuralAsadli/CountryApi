using CountryInfoApi.Abstractions.Repositories;
using CountryInfoApi.Abstractions.Services;
using CountryInfoApi.Dtos.City;
using CountryInfoApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using CountryInfoApi.Utilites.FiIeExtentions;

namespace CountryInfoApi.Service
{
    public class CityService : ICityService
    {
        public readonly IBaseRepository<City> _db;
        public readonly IWebHostEnvironment _hostingEnvironment;

        public CityService(IBaseRepository<City> db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
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
                    var fileName = item.RenameImg();
                    item.CreateImgFile(Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", fileName));


                    CityImg cityImg = new CityImg();
                    cityImg.ImgPath = fileName;
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
                    await _db.Remove(city);
                }
            };
        }

        public IEnumerable<City> GetAll()
        {
            
            return _db.GetAll(c => c.CityImgs);
        }

        public City GetById(Guid id)
        {
            
            return _db.Get(id, c => c.CityImgs);
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
                foreach (var img in existingObject.CityImgs)
                {
                    ImgExtention.DeleteImgFile(Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", img.ImgPath));
                }

                List<CityImg> cityImgs = new List<CityImg>();
                foreach (var img in cityDto.CityImgs)
                {
                    if (img.CheckImgFileType())
                    {
                        var fileName = img.RenameImg();
                        img.CreateImgFile(Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", fileName));


                        CityImg cityImg = new CityImg();
                        cityImg.ImgPath = fileName;
                        cityImgs.Add(cityImg);
                    }
                }
                existingObject.CityImgs = cityImgs;

                await _db.Update(existingObject);

            }
        }
    }
}
