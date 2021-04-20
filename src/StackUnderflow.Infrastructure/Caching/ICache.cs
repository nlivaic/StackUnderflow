using System;
using System.Threading.Tasks;

namespace StackUnderflow.Infrastructure.Caching
{
    public interface ICache
    {
        Task<T> GetOrCreate<T>(object key, Func<Task<T>> source, int seconds);
        void Remove(object key);
        void Set<T>(object key, T value, int seconds);
    }
}
