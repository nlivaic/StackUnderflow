using System;

namespace StackUnderflow.Core.Models
{
    public class AnswerGetModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public int VotesSum { get; set; }
    }
}
