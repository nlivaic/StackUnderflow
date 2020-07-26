using System;

namespace StackUnderflow.Core.Models
{
    public class CommentEditModel
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid? ParentAnswerId { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public string Body { get; set; }
    }
}
