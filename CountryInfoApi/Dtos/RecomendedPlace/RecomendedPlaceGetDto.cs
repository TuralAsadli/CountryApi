using CountryInfoApi.Dtos.City;
using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Dtos.RecomendedPlace
{
    public class RecomendedPlaceGetDto
    {
        public Guid Id { get; set; }
        [Required, MaxLength(30)]
        public string PlaceName { get; set; }

        [Required]
        public string Coordinates { get; set; }
        public string Description { get; set; }

        public GetCityDto City { get; set; }
        public IEnumerable<byte[]> PlacesImgs { get; set; }

        public IEnumerable<string> ImageBase64 { get; set; }
    }
}
