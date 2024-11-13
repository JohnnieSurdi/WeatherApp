namespace WeatherApp.Api
{
    public class WeatherApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _weatherUrl;
        private readonly string _geoUrl;

        public WeatherApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["WeatherApi:ApiKey"];
            _weatherUrl = configuration["WeatherApi:WeatherUrl"];
            _geoUrl = configuration["WeatherApi:GeoUrl"];
        }

        public async Task<string> GetWeatherDataAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{_weatherUrl}?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Network error occurred while fetching weather data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching weather data from API: " + ex.Message);
            }
        }
        public async Task<string> GetLocationDataAsync(string cityName)
        {
            try
            {
                var url = $"{_geoUrl}?q={cityName}&limit=1&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Network error occurred while fetching location data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching location data from API: " + ex.Message);
            }
        }
    }
}
