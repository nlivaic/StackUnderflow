using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Models
{
    public class QuestionModel
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public IEnumerable<TagModel> Tags { get; set; } = new List<TagModel>();
    }
}
