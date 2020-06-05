using System;

namespace StackUnderflow.Core.Models
{
    public class AnswerAcceptModel
    {
        public Guid QuestionOwnerId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}