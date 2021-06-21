using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Votes
{
    public interface IVoteService
    {
        Task ChangeCachedVotesSumAfterVoteCast(Vote vote);
        Task ChangeCachedVotesSumAfterVoteRevoked(Vote vote);
        IVoteable GetVoteable(Vote vote);
        Task<IVoteable> GetVoteableFromRepositoryAsync(VoteTargetEnum voteTarget, Guid voteTargetId);
        Task<int> GetVotesSumAsync(Guid targetId, VoteTargetEnum voteTarget);
    }
}