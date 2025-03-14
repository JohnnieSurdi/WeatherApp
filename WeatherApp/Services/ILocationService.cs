namespace WeatherApp.Services
{
    public interface ILocationService
    {
        Task<(double? lat, double? lon)> SearchByCityAsync(string cityName);
    }
}
