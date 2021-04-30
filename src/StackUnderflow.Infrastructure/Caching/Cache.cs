using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.Caching
{
    public class Cache : ICache
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores;

        public Cache(IMemoryCache cache)
        {
            _cache = cache;
            _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        public T Get<T>(string key) =>
            _cache.Get<T>(key);

        /// <summary>
        /// Simple implementation. In case two threads hit this method with the same key,
        /// it won't coordinate between the two and the source Func will be called twice.
        /// </summary>
        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> source, int seconds)
        {
            if (_cache.TryGetValue<T>(key, out T resultFromCache))
            {
                return resultFromCache;
            }
            T result = await source();
            Set(key, result, seconds);
            return result;
        }

        public async Task<int> IncrementAndGetConcurrent(string key, int seconds)
        {
            var cacheSemaphore = _semaphores.GetOrAdd(key, new SemaphoreSlim(1,1));
            await cacheSemaphore.WaitAsync();
            if (_cache.TryGetValue<int>(key, out int resultFromCache))
            {
                resultFromCache++;
                Set(key, resultFromCache, 60);
            }
            cacheSemaphore.Release();
            return resultFromCache;
        }

        /// <summary>
        /// Slower but thread-safe way to get and/or set a new value in cache.
        /// Modeled after https://bit.ly/3gppu8X.
        /// </summary>
        public async Task<T> GetOrCreateConcurrentAsync<T>(string key, Func<Task<T>> source, int seconds)
        {
            if (_cache.TryGetValue<T>(key, out T resultFromCache))
            {
                return resultFromCache;
            }
            SemaphoreSlim cacheSemaphore = _semaphores.GetOrAdd(key, new SemaphoreSlim(1, 1));
            T result;
            await cacheSemaphore.WaitAsync();
            try
            {
                // Try reading again to skip unneeded execution of source Func the second time.
                if (_cache.TryGetValue<T>(key, out T resultFromCacheWhileLocked))
                {
                    return resultFromCacheWhileLocked;
                }
                result = await source();
                Set(key, result, seconds);
            }
            finally
            {
                cacheSemaphore.Release();
            }
            return result;
        }

        public void Set<T>(string key, T value, int seconds)
        {
            _cache.Set<T>(key, value, new MemoryCacheEntryOptions
            {
                SlidingExpiration = new TimeSpan(0, seconds / 60, seconds % 60)
            });
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
