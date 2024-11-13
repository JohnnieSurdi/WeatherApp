using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApp.Api;
using WeatherApp.Mappers;
using WeatherApp.Models.Weather;
using WeatherApp.Logging;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly WeatherApi _weatherApi;
        private readonly IConfiguration _configuration;
        private readonly IWeatherLogger _logger;

        public WeatherService(WeatherApi weatherApi, IConfiguration configuration, IWeatherLogger logger)
        {
            _weatherApi = weatherApi;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                _logger.Info($"Fetching weather for coordinates: {latitude}, {longitude}");

                var content = await _weatherApi.GetWeatherDataAsync(latitude, longitude);
                var jsonResponse = JObject.Parse(content);
                return WeatherResponseMapper.MapFromJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error fetching weather data: {ex.Message}");
                throw new Exception("Error fetching weather data: " + ex.Message);
            }
        }

        public async Task<(double? lat, double? lon)> SearchByCityAsync(string cityName)
        {
            try
            {
                _logger.Info($"Fetching data for city: {cityName}");
                var response = await _weatherApi.GetLocationDataAsync(cityName);
                var location = JsonConvert.DeserializeObject<dynamic>(response);

                if (location.Count > 0)
                {
                    double latitude = location[0].lat;
                    double longitude = location[0].lon;

                    if (latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180)
                    {
                        return (latitude, longitude);
                    }
                }

                return (null, null);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error fetching data for city: {cityName}: {ex.Message}");
                throw new Exception("Error fetching location data: " + ex.Message);
            }
        }
    }
}
