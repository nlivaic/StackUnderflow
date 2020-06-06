using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IVoteService
    {
        Task CastVote(VoteCreateModel voteModel);
        Task RevokeVote(VoteRevokeModel voteModel);
    }
}