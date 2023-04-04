using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Dtos.City
{
    public class CityGetDto
    {
        [Required, MaxLength(30)]
        public string CityName { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public int Population { get; set; }
        public string Description { get; set; }
        
        public IEnumerable<byte[]> cityImgs { get; set; }
    }
}
