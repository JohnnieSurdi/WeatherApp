using Microsoft.Extensions.Caching.Memory;

namespace WeatherApp.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public T GetOrCreate<T>(string key, Func<ICacheEntry, T> createItem)
        {
            return _memoryCache.GetOrCreate(key, createItem);
        }

    }
}
