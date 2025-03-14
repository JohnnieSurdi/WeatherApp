using WeatherApp.Models.Weather;

namespace WeatherApp.Helpers
{
    public interface IWeatherDataJsonHandler
    {
        WeatherResponse ParseWeatherData(string jsonContent);
    }
}
