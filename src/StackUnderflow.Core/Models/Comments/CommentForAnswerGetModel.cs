using System;

namespace StackUnderflow.Core.Models
{
    public class CommentForAnswerGetModel
    {
        public Guid Id { get; set; }
        public Guid AnswerId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public int VotesSum { get; set; }
    }
}
