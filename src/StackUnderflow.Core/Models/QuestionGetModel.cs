using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Models
{
    public class QuestionGetModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public IEnumerable<CommentGetModel> Comments { get; set; } = new List<CommentGetModel>();
        public IEnumerable<TagGetModel> Tags { get; set; } = new List<TagGetModel>();
    }
}
