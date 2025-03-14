using WeatherApp.Models.Database;
using WeatherApp.Models;
using WeatherApp.Models.Weather;

namespace WeatherApp.Mappers.DBMappers
{
    public class WeatherSearchRecordMapper
    {
        public static WeatherSearchRecord MapFromWeatherResponse(WeatherResponse weatherResponse)
        {
            if (weatherResponse == null)
                throw new ArgumentNullException(nameof(weatherResponse));

            return new WeatherSearchRecord
            {
                Id = Guid.NewGuid().ToString(),
                CityName = weatherResponse.Location.CityName,
                CountryName = weatherResponse.Location.CountryName,
                Lon = weatherResponse.Location.Lon,
                Lat = weatherResponse.Location.Lat,
                Main = weatherResponse.Weather.Main,
                Description = weatherResponse.Weather.Description,
                Temp = weatherResponse.Weather.Temp,
                Pressure = weatherResponse.Weather.Pressure,
                Humidity = weatherResponse.Weather.Humidity,
                Speed = weatherResponse.Weather.Speed,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
