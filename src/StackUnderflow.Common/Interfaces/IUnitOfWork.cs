using System.Threading.Tasks;

namespace StackUnderflow.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}