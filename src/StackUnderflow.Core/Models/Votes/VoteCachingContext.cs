using System;

namespace StackUnderflow.Core.Models.Votes
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
