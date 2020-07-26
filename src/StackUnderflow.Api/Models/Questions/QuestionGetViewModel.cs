using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Models
{
    public class QuestionGetViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public string CreatedOn { get; set; }
        public int VotesSum { get; set; }
        public IEnumerable<TagGetViewModel> Tags { get; set; } = new List<TagGetViewModel>();
    }
}
