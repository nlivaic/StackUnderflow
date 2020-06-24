using System;

namespace StackUnderflow.Core.Entities
{
    public class QuestionTag
    {
        public Question Question { get; set; }
        public Guid QuestionId { get; set; }
        public Tag Tag { get; set; }
        public Guid TagId { get; set; }
    }
}