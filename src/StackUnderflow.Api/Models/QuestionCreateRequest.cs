using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Models
{
    public class QuestionCreateRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}