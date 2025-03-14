using WeatherApp.Models.Weather;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude);
    }
}
