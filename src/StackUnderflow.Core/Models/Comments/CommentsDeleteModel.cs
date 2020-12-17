using System;

namespace StackUnderflow.Core.Models
{
    public class CommentsDeleteModel
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid? ParentAnswerId { get; set; }
    }
}
