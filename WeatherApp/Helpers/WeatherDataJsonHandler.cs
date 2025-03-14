using Newtonsoft.Json.Linq;
using WeatherApp.Mappers;
using WeatherApp.Models.Weather;

namespace WeatherApp.Helpers
{
    public class WeatherDataJsonHandler : IWeatherDataJsonHandler
    {
        public WeatherResponse ParseWeatherData(string jsonContent)
        {
            var jsonResponse = JObject.Parse(jsonContent);
            return WeatherResponseMapper.MapFromJson(jsonResponse);
        }
    }
}
