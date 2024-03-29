using System.Collections.Generic;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface IVoteable
    {
        IEnumerable<Vote> Votes { get; }

        void ApplyVote(Vote vote);
        void RevokeVote(Vote vote, ILimits limits);
    }
}