using StackUnderflow.Core.Enums;
using System;
using static StackUnderflow.Core.Entities.Vote;

namespace StackUnderflow.Core.Models
{
    public class VoteCreateModel
    {
        public Guid UserId { get; set; }
        public Guid TargetId { get; set; }
        public VoteTargetEnum VoteTarget { get; set; }
        public VoteTypeEnum VoteType { get; set; }
    }
}