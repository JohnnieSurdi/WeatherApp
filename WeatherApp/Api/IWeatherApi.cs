namespace WeatherApp.Api
{
    public interface IWeatherApi
    {
        Task<string> GetWeatherDataAsync(double latitude, double longitude);
        Task<string> GetLocationDataAsync(string cityName);
    }
}
