using System;

namespace StackUnderflow.Application.Comments.Models
{
    public class CommentDeleteModel
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid? ParentAnswerId { get; set; }
        public Guid CommentId { get; set; }
    }
}
