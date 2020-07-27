using System;

namespace StackUnderflow.Core.Models
{
    public class CommentDeleteModel
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid? ParentAnswerId { get; set; }
        public Guid CommentId { get; set; }
    }
}
