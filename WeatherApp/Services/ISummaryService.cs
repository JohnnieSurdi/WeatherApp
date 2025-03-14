namespace WeatherApp.Services
{
    public interface ISummaryService
    {
        Task<string> GenerateSummaryAsync(string cityName);
    }
}
