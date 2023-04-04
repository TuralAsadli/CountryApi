using CountryInfoApi.Dtos.CurrentInfo;

namespace CountryInfoApi.Dtos.ForecastInfo
{
    public class ForecastInfoDto
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public ForecastDto forecast { get; set; }
    }
}
