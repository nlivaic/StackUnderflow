using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Models
{
    public class QuestionCreateModel
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}
