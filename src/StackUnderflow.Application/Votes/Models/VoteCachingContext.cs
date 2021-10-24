using System;
using StackUnderflow.Core.Enums;

namespace StackUnderflow.Application.Votes.Models
{
    public class VoteCachingContext
    {
        public VoteCachingContext(Guid targetId, VoteTargetEnum target)
        {
            TargetId = targetId;
            Target = target;
        }

        public Guid TargetId { get; set; }
        public VoteTargetEnum Target { get; set; }
    }
}
