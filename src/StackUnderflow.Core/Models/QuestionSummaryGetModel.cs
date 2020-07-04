using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Models
{
    public class QuestionSummaryGetModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortBody { get; set; }
        public string Username { get; set; }
        public string CreatedOn { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public IEnumerable<TagGetModel> Tags { get; set; } = new List<TagGetModel>();
        public int AnswersCount { get; set; }
        public int VotesSum { get; set; }
    }
}
