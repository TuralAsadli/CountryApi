using CountryInfoApi.Models.Base;

namespace CountryInfoApi.Models
{
    public class City : BaseItem
    {
        
        public string CityName { get; set; }

        public string Area { get; set; }

        public int Population { get; set; }

        public string Description { get; set; }

        public IEnumerable<CityImg> CityImgs { get; set; }
        public IEnumerable<RecomendedPlace> RecomendedPlaces { get; set; }

    }
}
