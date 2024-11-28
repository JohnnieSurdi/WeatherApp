using Amazon.DynamoDBv2.DataModel;
using WeatherApp.Models.Database;

namespace WeatherApp.Repositories
{
    public class WeatherSearchRepository : IWeatherSearchRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public WeatherSearchRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task SaveWeatherSearchRecordAsync(WeatherSearchRecord record)
        {
            await _dynamoDbContext.SaveAsync(record);
        }

        public async Task<IEnumerable<WeatherSearchRecord>> GetRecentWeatherSearchesAsync()
        {
            var result = await _dynamoDbContext.ScanAsync<WeatherSearchRecord>(null).GetRemainingAsync();
            Console.WriteLine($"Found {result.Count} results.");

            return result;
        }

    }
}
