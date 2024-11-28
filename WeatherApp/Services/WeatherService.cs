using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApp.Api;
using WeatherApp.Mappers;
using WeatherApp.Models.Weather;
using WeatherApp.Logging;
using WeatherApp.Repositories;
using WeatherApp.Mappers.DBMappers;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApi _weatherApi;
        private readonly WeatherApp.Logging.ILogger _logger;
        private readonly IWeatherSearchRepository _weatherRepository;
        private readonly ICacheService _cacheService;

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);

        public WeatherService(WeatherApi weatherApi, WeatherApp.Logging.ILogger logger, IWeatherSearchRepository weatherRepository, ICacheService cacheService)
        {
            _weatherApi = weatherApi;
            _logger = logger;
            _weatherRepository = weatherRepository;
            _cacheService = cacheService;
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            string cacheKey = $"weather:{latitude},{longitude}";

            return await _cacheService.GetCachedOrFetchAsync(
                cacheKey,
                async () =>
                    {
                        _logger.Info($"Fetching weather data for coordinates: {latitude}, {longitude}");

                        var content = await _weatherApi.GetWeatherDataAsync(latitude, longitude);
                        var jsonResponse = JObject.Parse(content);

                        var weatherResponse = WeatherResponseMapper.MapFromJson(jsonResponse);

                        var record = WeatherSearchRecordMapper.MapFromWeatherResponse(weatherResponse);
                        _weatherRepository.SaveWeatherSearchRecordAsync(record).Wait();

                        return weatherResponse;
                    },
                    _cacheDuration
            );
        }

        public async Task<(double? lat, double? lon)> SearchByCityAsync(string cityName)
        {
                var cacheKey = $"coords-{cityName.ToLower()}";

                return await _cacheService.GetCachedOrFetchAsync(cacheKey, async () =>
                    {
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
                        throw new KeyNotFoundException($"No coordinates found for city: {cityName}");
                    },
                    _cacheDuration
                    ); 
        }
    }
}
