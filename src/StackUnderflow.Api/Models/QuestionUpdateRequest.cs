using System;
using System.Collections.Generic;

namespace StackUnderflow.Api.Models
{
    public class QuestionUpdateRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}