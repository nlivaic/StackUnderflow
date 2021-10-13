using System;
using System.Collections.Generic;

namespace StackUnderflow.Application.Questions.Models
{
    public class QuestionEditModel
    {
        public Guid QuestionUserId { get; set; }
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}