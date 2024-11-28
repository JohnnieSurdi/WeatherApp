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

        public async Task<T> GetCachedOrFetchAsync<T>(string key, Func<Task<T>> fetchFunc, TimeSpan cacheDuration)
        {
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var data = await fetchFunc();
            _memoryCache.Set(key, data, cacheDuration);
            return data;
        }
    }
}
