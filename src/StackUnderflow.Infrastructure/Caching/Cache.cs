using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.Caching
{
    public class Cache : ICache
    {
        private readonly IMemoryCache _cache;

        public Cache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get<T>(object key, Func<Task<T>> source, int seconds)
        {
            if (_cache.TryGetValue<T>(key, out T resultFromCache))
            {
                return resultFromCache;
            }
            T result = await source();
            Set(key, result, seconds);
            return result;
        }

        public void Set<T>(object key, T value, int seconds)
        {
            _cache.Set<T>(key, value, new MemoryCacheEntryOptions
            {
                SlidingExpiration = new TimeSpan(0, seconds / 60, seconds % 60)
            });
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }
    }
}
