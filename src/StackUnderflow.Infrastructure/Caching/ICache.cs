using System;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.Caching
{
    public interface ICache
    {
        Task<int> DecrementAndGetConcurrentAsync(string key, Func<Task<int>> source, int seconds);
        T Get<T>(string key);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> source, int seconds);
        Task<T> GetOrCreateConcurrentAsync<T>(string key, Func<Task<T>> source, int seconds);
        Task<int> IncrementAndGetConcurrentAsync(string key, Func<Task<int>> source, int seconds);
        void Remove(string key);
        void Set<T>(string key, T value, int seconds);
    }
}
