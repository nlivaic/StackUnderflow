using System;

namespace StackUnderflow.Application.Comments.Models
{
    public class CommentOnAnswerCreateModel
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; }
    }
}
