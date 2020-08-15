using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<Vote> GetVoteAsync(Guid voteUserId, Guid voteId);
        Task<Vote> GetVoteAsync(Guid voteUserId, Guid? questionId, Guid? answerId, Guid? commentId);
    }
}