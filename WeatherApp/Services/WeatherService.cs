using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApp.Api;
using WeatherApp.Mappers;
using WeatherApp.Models.Weather;
using WeatherApp.Logging;
using WeatherApp.Repositories;
using WeatherApp.Mappers.DBMappers;
using WeatherApp.Models.Location;
using WeatherApp.Helpers;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApi _weatherApi;
        private readonly WeatherApp.Logging.ILogger _logger;
        private readonly IWeatherSearchRepository _weatherRepository;
        private readonly ICacheService _cacheService;
        private readonly IWeatherDataJsonHandler _weatherDataJsonHandler;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);

        public WeatherService(IWeatherApi weatherApi, WeatherApp.Logging.ILogger logger, IWeatherSearchRepository weatherRepository, ICacheService cacheService, IWeatherDataJsonHandler weatherDataJsonHandler)
        {
            _weatherApi = weatherApi;
            _logger = logger;
            _weatherRepository = weatherRepository;
            _cacheService = cacheService;
            _weatherDataJsonHandler = weatherDataJsonHandler;
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
                        var weatherResponse = _weatherDataJsonHandler.ParseWeatherData(content);
                        await _weatherRepository.SaveWeatherSearchRecordAsync(WeatherSearchRecordMapper.MapFromWeatherResponse(weatherResponse));
                        return weatherResponse;
                    },
                    _cacheDuration
            );
        }
    }
}
