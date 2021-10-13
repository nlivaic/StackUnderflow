using System;

namespace StackUnderflow.Application.Answers.Models
{
    public class AnswerCreateModel
    {
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; }
    }
}