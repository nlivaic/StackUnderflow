using AutoMapper;
using System;

namespace StackUnderflow.Application.Answers.Models
{
    public class AnswerEditModel
    {
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string Body { get; set; }
    }
}