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
                CityName = weatherResponse.CityName,
                CountryName = weatherResponse.CountryName,
                Lon = weatherResponse.Lon,
                Lat = weatherResponse.Lat,
                Main = weatherResponse.Main,
                Description = weatherResponse.Description,
                Temp = weatherResponse.Temp,
                Pressure = weatherResponse.Pressure,
                Humidity = weatherResponse.Humidity,
                Speed = weatherResponse.Speed,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
