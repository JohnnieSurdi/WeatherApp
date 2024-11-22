using WeatherApp.Models.Database;

namespace WeatherApp.Repositories
{
    public interface IWeatherSearchRepository
    {
        Task SaveWeatherSearchRecordAsync(WeatherSearchRecord record);
    }
}
