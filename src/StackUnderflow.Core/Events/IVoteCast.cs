using System;
using StackUnderflow.Core.Enums;

namespace StackUnderflow.Core.Events
{
    public interface IVoteCast
    {
        public Guid UserId { get; set; }
        public VoteTypeEnum VoteType { get; set; }
    }
}
