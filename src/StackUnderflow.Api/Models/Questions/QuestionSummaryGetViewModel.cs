using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Models
{
    public class QuestionSummaryGetViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string CreatedOn { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public int Answers { get; set; }
        public int VotesSum { get; set; }
    }
}
