using System.Collections.Generic;

namespace StackUnderflow.Core.Models
{
    public class QuestionSummaryGetModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string CreatedOn { get; set; }
        public bool HasAcceptedAnswer { get; set; }
        public IEnumerable<TagGetModel> Tags { get; set; } = new List<TagGetModel>();
        public int Answers { get; set; }
        public int VotesSum { get; set; }
    }
}
