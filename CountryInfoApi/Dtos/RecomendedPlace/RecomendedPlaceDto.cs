using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Dtos.RecomendedPlace
{
    public class RecomendedPlaceDto
    {
        [Required,MaxLength(30)]
        public string PlaceName { get; set; }

        [Required]
        public string Coordinates { get; set; }
        public string Description { get; set; }


        public IEnumerable<IFormFile> PlacesImgsFormFile { get; set; }
    }
}
