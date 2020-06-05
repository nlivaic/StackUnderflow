using System;

namespace StackUnderflow.Core.Models
{
    public class CommentCreateModel
    {
        public Guid QuestionId { get; set; }
        public Guid OwnerId { get; set; }
        public string Body { get; set; }
    }
}
