using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.PointServices
{
    public class PointService : IPointService
    {
        private readonly BaseLimits _limits;
        private readonly IUserRepository _userRepository;

        public PointService(
            BaseLimits limits,
            IUserRepository userRepository)
        {
            _limits = limits;
            _userRepository = userRepository;
        }

        public async Task CalculateAsync(Guid userId, VoteTypeEnum voteType)
        {
            await _userRepository.CalculatePointsAsync(
                userId,
                voteType == VoteTypeEnum.Upvote
                    ? _limits.UpvotePoints
                    : _limits.DownvotePoints);
        }
    }
}
