using Microsoft.Extensions.Caching.Memory;

namespace WeatherApp.Services
{
    public interface ICacheService
    {
        Task<T> GetCachedOrFetchAsync<T>(string key, Func<Task<T>> fetchFunc, TimeSpan cacheDuration);
    }
}
