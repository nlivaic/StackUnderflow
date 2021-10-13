using System;

namespace StackUnderflow.Application.Comments.Models
{
    public class CommentsDeleteModel
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid? ParentAnswerId { get; set; }
    }
}
