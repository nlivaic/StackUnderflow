using StackUnderflow.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Models
{
    public class Answer : BaseEntity<Guid>
    {
        public string Body { get; private set; }
        public bool IsAcceptedAnswer { get; private set; }
        public DateTime? AcceptedOn { get; private set; }
        public Question Question { get; private set; }
        public Guid QuestionId { get; private set; }
        public IEnumerable<Comment> Comments => _comments;

        private List<Comment> _comments;

        public Answer(string body, Guid questionId, IEnumerable<Comment> comments)
        {
            Body = body ?? throw new ArgumentException("Answer must have a body.");
            QuestionId = questionId == Guid.Empty
                ? throw new ArgumentException("Answer must relate to a question.")
                : questionId;
            _comments = comments == null || comments.Count() == 0
                ? new List<Comment>()
                : new List<Comment>(comments);
        }

        public void AcceptedAnswer()
        {
            IsAcceptedAnswer = true;
            AcceptedOn = DateTime.UtcNow;
            // @nl: Raise an event!
        }
    }
}