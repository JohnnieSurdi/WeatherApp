using Newtonsoft.Json.Linq;
using WeatherApp.Models.Weather;

namespace WeatherApp.Mappers
{
    public class WeatherResponseMapper
    {
        public static WeatherResponse MapFromJson(JObject json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            var weatherArray = json.SelectToken("weather") as JArray;
            var weatherMain = string.Empty;
            var weatherDescription = string.Empty;

            if (weatherArray != null && weatherArray.Count > 0)
            {
                var firstWeather = weatherArray[0];
                weatherMain = firstWeather["main"]?.ToString() ?? string.Empty;
                weatherDescription = firstWeather["description"]?.ToString() ?? string.Empty;
            }

            return new WeatherResponse
            {
                Location = new LocationData
                {
                    CityName = json.Value<string>("name") ?? string.Empty,
                    CountryName = json.SelectToken("sys.country")?.ToString() ?? string.Empty,
                    Lon = json.SelectToken("coord.lon")?.Value<double>() ?? 0,
                    Lat = json.SelectToken("coord.lat")?.Value<double>() ?? 0
                },
                Weather = new WeatherData
                {
                    Main = weatherMain,
                    Description = weatherDescription,
                    Temp = json.SelectToken("main.temp")?.Value<double>() ?? 0,
                    Pressure = json.SelectToken("main.pressure")?.Value<int>() ?? 0,
                    Humidity = json.SelectToken("main.humidity")?.Value<int>() ?? 0,
                    Speed = json.SelectToken("wind.speed")?.Value<double>() ?? 0
                }
            };
        }
    }
}
