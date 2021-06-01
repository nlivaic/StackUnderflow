using StackUnderflow.Core.Enums;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Core.Interfaces
{
    public interface IPointService
    {
        Task CalculateAsync(Guid userId, VoteTypeEnum voteType);
    }
}
