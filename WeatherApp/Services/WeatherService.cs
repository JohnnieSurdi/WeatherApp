using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApp.Api;
using WeatherApp.Mappers;
using WeatherApp.Models.Weather;
using WeatherApp.Logging;
using WeatherApp.Repositories;
using WeatherApp.Mappers.DBMappers;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApi _weatherApi;
        private readonly IWeatherLogger _logger;
        private readonly IWeatherSearchRepository _weatherRepository;
        private readonly ICacheService _cacheService;

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);
        private readonly (double, double) londonCoords = (51.4875167, -0.1687007);

        public WeatherService(WeatherApi weatherApi, IWeatherLogger logger, IWeatherSearchRepository weatherRepository, ICacheService cacheService)
        {
            _weatherApi = weatherApi;
            _logger = logger;
            _weatherRepository = weatherRepository;
            _cacheService = cacheService;
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            string cacheKey = $"weather:{latitude},{longitude}";

            return _cacheService.GetOrCreate(
                cacheKey,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

                    try
                    {
                        _logger.Info($"Fetching weather data for coordinates: {latitude}, {longitude}");

                        var content = _weatherApi.GetWeatherDataAsync(latitude, longitude).Result;
                        var jsonResponse = JObject.Parse(content);

                        var weatherResponse = WeatherResponseMapper.MapFromJson(jsonResponse);

                        var record = WeatherSearchRecordMapper.MapFromWeatherResponse(weatherResponse);
                        _weatherRepository.SaveWeatherSearchRecordAsync(record).Wait();

                        return weatherResponse;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Error fetching weather data for coordinates {latitude},{longitude}: {ex.Message}");
                        throw new Exception("Error fetching weather data", ex);
                    }
                });
        }

        public async Task<(double? lat, double? lon)> SearchByCityAsync(string cityName)
        {
            try
            {
                _logger.Info($"Fetching data for city: {cityName}");

                var cacheKey = $"coords-{cityName.ToLower()}";

                return await _cacheService.GetOrCreate(cacheKey, async entry =>
                    {
                        entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

                        _logger.Info($"Fetching coordinates for city: {cityName}");

                        var response = await _weatherApi.GetLocationDataAsync(cityName);
                        var location = JsonConvert.DeserializeObject<dynamic>(response);

                        if (location.Count > 0)
                        {
                            double latitude = location[0].lat;
                            double longitude = location[0].lon;

                            if (latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180)
                            {
                                _logger.Info($"Found coordinates for city {cityName}: {latitude}, {longitude}");
                                return (latitude, longitude);
                            }
                        }

                        _logger.Warn($"No valid coordinates found for city: {cityName}. Instead look at London's weather: ");
                        return londonCoords;
                    });

            }

            catch (Exception ex)
            {
                _logger.Error($"Unexpected error while fetching data for city {cityName}: {ex.Message}");
                throw new Exception("Unexpected error fetching location data.", ex);
            }
        }
    }
}
