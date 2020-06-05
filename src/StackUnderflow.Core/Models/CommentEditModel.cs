using System;

namespace StackUnderflow.Core.Models
{
    public class CommentEditModel
    {
        public Guid OwnerId { get; set; }
        public Guid CommentId { get; set; }
        public string Body { get; set; }
    }
}
