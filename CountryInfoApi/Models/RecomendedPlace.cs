using CountryInfoApi.Models.Base;

namespace CountryInfoApi.Models
{
    public class RecomendedPlace : BaseItem
    {
        public string PlaceName { get; set; }

        public string Coordinates { get; set; }

        public string Description { get; set; }

        public Guid CityId { get; set; }
        public City City { get; set; }

        public IEnumerable<PlaceImg> PlaceImgs { get; set; }
    }
}
