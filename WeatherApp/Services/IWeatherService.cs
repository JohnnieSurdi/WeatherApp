using WeatherApp.Models.Weather;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude);
        Task<(double? lat, double? lon)> SearchByCityAsync(string cityName);
    }
}
