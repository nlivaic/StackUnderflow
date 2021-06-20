using System;

namespace StackUnderflow.Core.Models
{
    public class CommentForAnswerGetModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public bool IsOwner { get; set; }
        public bool IsModerator { get; set; }
    }
}
