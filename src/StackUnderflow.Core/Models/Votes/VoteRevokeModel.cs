using System;
using static StackUnderflow.Core.Entities.Vote;

namespace StackUnderflow.Core.Models
{
    public class VoteRevokeModel
    {
        public Guid UserId { get; set; }
        public Guid VoteId { get; set; }
    }
}