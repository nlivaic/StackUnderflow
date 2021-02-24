using System;

namespace StackUnderflow.Api.Models
{
    public class CommentForAnswerGetViewModel : IOwneable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AnswerId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public bool IsOwner { get; set; }
        public bool IsModerator { get; set; }
    }
}
