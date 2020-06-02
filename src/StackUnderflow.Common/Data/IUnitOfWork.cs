using System.Threading.Tasks;

namespace StackUnderflow.Common.Query
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}