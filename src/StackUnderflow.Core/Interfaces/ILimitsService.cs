using System.Threading.Tasks;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface ILimitsService
    {
        Task<Limits> Get();
    }
}
