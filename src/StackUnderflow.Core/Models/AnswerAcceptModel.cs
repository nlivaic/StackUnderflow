using System;

namespace StackUnderflow.Core.Models
{
    public class AnswerAcceptModel
    {
        public Guid QuestionUserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}