using StackUnderflow.Core.Enums;
using System;

namespace StackUnderflow.Application.Votes.Models
{
    public class VoteCachingContext
    {
        public Guid TargetId { get; set; }
        public VoteTargetEnum Target { get; set; }

        public VoteCachingContext(Guid targetId, VoteTargetEnum target)
        {
            TargetId = targetId;
            Target = target;
        }
    }
}
