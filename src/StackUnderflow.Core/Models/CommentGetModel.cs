using System;

namespace StackUnderflow.Core.Models
{
    public class CommentGetModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
    }
}
