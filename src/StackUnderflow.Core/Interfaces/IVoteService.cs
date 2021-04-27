using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Votes;

namespace StackUnderflow.Core.Interfaces
{
    public interface IVoteService
    {
        Task<VoteGetModel> CastVoteAsync(VoteCreateModel voteModel);
        Task RevokeVoteAsync(VoteRevokeModel voteModel);
        Task<VoteGetModel> GetVoteAsync(Guid voteId);
    }
}