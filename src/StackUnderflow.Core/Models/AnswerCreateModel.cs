using System;

namespace StackUnderflow.Core.Models
{
    public class AnswerCreateModel
    {
        public Guid QuestionId { get; set; }
        public Guid OwnerId { get; set; }
        public string Body { get; set; }
    }
}