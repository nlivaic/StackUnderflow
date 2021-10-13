using System;

namespace StackUnderflow.Application.Answers.Models
{
    public class AnswerAcceptModel
    {
        public Guid QuestionUserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}