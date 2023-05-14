using CountryInfoApi.Dtos.ForecastInfo;

namespace CountryInfoApi.Dtos.City
{
    public class CityDtoWithForecast
    {
        public ForecastInfoDto ForecastDto { get; set; }
        public GetCityDto City { get; set; }
    }
}
