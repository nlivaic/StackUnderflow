using System;

namespace StackUnderflow.Application.Votes.Models
{
    public class VoteRevokeModel
    {
        public Guid UserId { get; set; }
        public Guid VoteId { get; set; }
    }
}