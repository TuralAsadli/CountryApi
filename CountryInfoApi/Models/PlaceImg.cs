using CountryInfoApi.Models.Base;

namespace CountryInfoApi.Models
{
    public class PlaceImg : BaseItem
    {
        public string ImgPath { get; set; }
        public Guid RecomendedPlaceId { get; set; }
        public RecomendedPlace RecomendedPlace { get; set; }
    }
}
