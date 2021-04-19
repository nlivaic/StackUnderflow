using StackUnderflow.Core.Entities;
using System.Threading.Tasks;

namespace StackUnderflow.Core.Interfaces
{
    public interface ILimitsService
    {
        Task<Limits> Get();
    }
}
