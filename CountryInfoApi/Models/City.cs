using CountryInfoApi.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CountryInfoApi.Models
{
    public class City : BaseItem
    {
        [MaxLength(50)]
        public string CityName { get; set; }
        [MaxLength(50)]
        public string Area { get; set; }

        public int Population { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public IEnumerable<CityImg> CityImgs { get; set; }
        public IEnumerable<RecomendedPlace> RecomendedPlaces { get; set; }

    }
}
