
using Newtonsoft.Json;
using WeatherApp.Api;
using WeatherApp.Models.Location;

namespace WeatherApp.Services
{
    public class LocationService : ILocationService
    {
        private readonly IWeatherApi _weatherApi;
        private readonly WeatherApp.Logging.ILogger _logger;
        private readonly ICacheService _cacheService;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);

        public LocationService(IWeatherApi weatherApi, WeatherApp.Logging.ILogger logger, ICacheService cacheService)
        {
            _weatherApi = weatherApi;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<(double? lat, double? lon)> SearchByCityAsync(string cityName)
        {
            var cacheKey = $"coords-{cityName.ToLower()}";
            return await _cacheService.GetCachedOrFetchAsync(cacheKey, async () =>
            {
                _logger.Info($"Fetching coordinates for city: {cityName}");
                var response = await _weatherApi.GetLocationDataAsync(cityName);
                var locations = JsonConvert.DeserializeObject<List<LocationResponse>>(response);

                if (locations != null && locations.Count > 0)
                {
                    var location = locations[0];
                    double latitude = location.Lat;
                    double longitude = location.Lon;

                    if (latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180)
                    {
                        _logger.Info($"Found coordinates for city {cityName}: {latitude}, {longitude}");
                        return (latitude, longitude);
                    }
                }
                throw new ArgumentException($"No coordinates found for city: {cityName}");
            }, _cacheDuration);
        }
    }
}
