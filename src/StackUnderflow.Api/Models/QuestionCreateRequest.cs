using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Models
{
    public class QuestionCreateRequest
    {
        public string Body { get; set; }
        public string Title { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}