using CountryInfoApi.Dtos.RecomendedPlace;
using CountryInfoApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Dtos.City
{
    public class GetCityDto
    {
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string CityName { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public int Population { get; set; }
        public string Description { get; set; }
        
        public IEnumerable<string> cityImgs { get; set; }

        public IEnumerable<RecomendedPlaceGetDto> Places { get; set; }
    }
}
