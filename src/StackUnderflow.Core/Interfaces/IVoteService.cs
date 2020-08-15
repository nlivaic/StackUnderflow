using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IVoteService
    {
        Task CastVoteAsync(VoteCreateModel voteModel);
        Task RevokeVoteAsync(VoteRevokeModel voteModel);
    }
}