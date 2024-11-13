using Newtonsoft.Json.Linq;
using WeatherApp.Models.Weather.WeatherResponseData;
using WeatherApp.Models.Weather;

namespace WeatherApp.Mappers
{
    public class WeatherResponseMapper
    {
        public static WeatherResponse MapFromJson(JObject json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            return new WeatherResponse
            {
                CityName = json.Value<string>("name") ?? string.Empty,
                CountryName = json.SelectToken("sys.country")?.ToString() ?? string.Empty,
                Coord = new Coord
                {
                    Lon = json.SelectToken("coord.lon")?.Value<double>() ?? 0,
                    Lat = json.SelectToken("coord.lat")?.Value<double>() ?? 0
                },
                Weather = json["weather"] is JArray weatherArray && weatherArray.Count > 0
                    ? new WeatherData
                    {
                        Main = weatherArray[0].Value<string>("main") ?? string.Empty,
                        Description = weatherArray[0].Value<string>("description") ?? string.Empty
                    }
                    : new WeatherData(),
                Main = new Main
                {
                    Temp = json.SelectToken("main.temp")?.Value<double>() ?? 0,
                    Pressure = json.SelectToken("main.pressure")?.Value<int>() ?? 0,
                    Humidity = json.SelectToken("main.humidity")?.Value<int>() ?? 0
                },
                Wind = new Wind
                {
                    Speed = json.SelectToken("wind.speed")?.Value<double>() ?? 0
                }
            };
        }
    }
}
