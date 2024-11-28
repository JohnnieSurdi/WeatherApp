using WeatherApp.Helpers;

namespace WeatherApp.Api
{
    public class WeatherApi
    {
        private readonly ApiRequestHandler _apiRequestHandler;
        private readonly string _apiKey;
        private readonly string _weatherUrl;
        private readonly string _geoUrl;

        public WeatherApi(ApiRequestHandler apiRequestHandler, IConfiguration configuration)
        {
            _apiRequestHandler = apiRequestHandler;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _weatherUrl = configuration["WeatherApi:WeatherUrl"];
            _geoUrl = configuration["WeatherApi:GeoUrl"];
        }

        public async Task<string> GetWeatherDataAsync(double latitude, double longitude)
        {
            var url = $"{_weatherUrl}?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";
            return await _apiRequestHandler.ExecuteApiCallAsync(url);
        }
        public async Task<string> GetLocationDataAsync(string cityName)
        {
            var url = $"{_geoUrl}?q={cityName}&limit=1&appid={_apiKey}";
            return await _apiRequestHandler.ExecuteApiCallAsync(url);
        }
    }
}
