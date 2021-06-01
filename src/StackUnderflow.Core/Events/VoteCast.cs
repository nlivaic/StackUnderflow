using StackUnderflow.Core.Enums;
using System;

namespace StackUnderflow.Core.Events
{
    public interface VoteCast
    {
        public Guid UserId { get; set; }
        public VoteTypeEnum VoteType { get; set; }
    }
}
