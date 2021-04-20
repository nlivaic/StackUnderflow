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


        /// <summary>
        /// Simple implementation. In case two threads hit this method with the same source Func,
        /// it won't coordinate between the two, so some resources might be wasted.
        /// In case that is an issue, take a look at https://bit.ly/3gppu8X for a thread-safe approach.
        /// </summary>
        public async Task<T> GetOrCreate<T>(object key, Func<Task<T>> source, int seconds)
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
