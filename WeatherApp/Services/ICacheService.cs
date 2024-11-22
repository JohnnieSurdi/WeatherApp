using Microsoft.Extensions.Caching.Memory;

namespace WeatherApp.Services
{
    public interface ICacheService
    {
        T GetOrCreate<T>(string key, Func<ICacheEntry, T> createItem);
    }
}
