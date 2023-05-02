using CountryInfoApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Dtos.City
{
    public class CityDto
    {
        public string CityName { get; set; }

        public string Area { get; set; }

        public int Population { get; set; }
        public string Description { get; set; }
        public IEnumerable<IFormFile> CityImgsFormFile { get; set; }
    }
}
