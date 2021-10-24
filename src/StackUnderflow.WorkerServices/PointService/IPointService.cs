using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Enums;

namespace StackUnderflow.WorkerServices.PointServices
{
    public interface IPointService
    {
        Task CalculateAsync(Guid userId, VoteTypeEnum voteType);
    }
}
