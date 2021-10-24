using System;
using StackUnderflow.Core.Enums;

namespace StackUnderflow.Application.Votes.Models
{
    public class VoteCreateModel
    {
        public Guid UserId { get; set; }
        public Guid TargetId { get; set; }
        public VoteTargetEnum VoteTarget { get; set; }
        public VoteTypeEnum VoteType { get; set; }
    }
}