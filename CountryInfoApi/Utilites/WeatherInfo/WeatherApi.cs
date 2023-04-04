using CountryInfoApi.Dtos.CurrentInfo;
using CountryInfoApi.Dtos.ForecastInfo;
using Newtonsoft.Json;

namespace CountryInfoApi.Utilites.WeatherInfo
{
    public class WeatherApi
    {
        private static readonly string Key = "1d123e55e8794dffab785956232803";

        public static async Task<CurrentInfoDto> GetCurrentInfo(string CityName)
        {
            HttpClient client = new HttpClient();
            try
            {
                var ApiRes = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={Key}&q={CityName}&aqi=no");
                CurrentInfoDto currentInfo = JsonConvert.DeserializeObject<CurrentInfoDto>(ApiRes);
                return currentInfo;
            }
            catch (Exception)
            {
                return null;
            }
            
        } 

        public static async Task<ForecastInfoDto> GetForecastInfo(string CityName, int days)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                var ApiRes = await httpClient.GetStringAsync($"https://api.weatherapi.com/v1/forecast.json?key={Key}&q={CityName}&days={days}&aqi=no&alerts=no");
                ForecastInfoDto currentInfo = JsonConvert.DeserializeObject<ForecastInfoDto>(ApiRes);
                return currentInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
