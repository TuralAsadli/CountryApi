using CountryInfoApi.Models.Base;

namespace CountryInfoApi.Models
{
    public class CityImg : BaseItem
    {
        public string ImgPath { get; set; }


        public Guid CityId { get; set; }
        public City City { get; set; }
    }
}
