using System;

namespace StackUnderflow.Core.Models
{
    public class AnswerEditModel
    {
        public Guid OwnerId { get; set; }
        public Guid AnswerId { get; set; }
        public string Body { get; set; }
    }
}