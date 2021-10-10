using StackUnderflow.Core.Enums;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Services
{
    public interface IPointService
    {
        Task CalculateAsync(Guid userId, VoteTypeEnum voteType);
    }
}
