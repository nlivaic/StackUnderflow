using StackUnderflow.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Entities
{
    public class Answer : BaseEntity<Guid>
    {
        public Guid OwnerId { get; private set; }
        public string Body { get; private set; }
        public bool IsAcceptedAnswer { get; private set; }
        public DateTime? AcceptedOn { get; private set; }
        public Question Question { get; private set; }
        public Guid QuestionId { get; private set; }
        public IEnumerable<Comment> Comments => _comments;

        private List<Comment> _comments;

        public Answer(Guid id)
        {
            Id = id;
        }

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
        }

        public static Answer Create(Guid ownerId, string body, Question question)
        {
            var answer = new Answer(Guid.NewGuid());
            answer.OwnerId = ownerId;
            answer.Body = body;
            answer.IsAcceptedAnswer = false;
            answer.Question = question;
            answer.QuestionId = question.Id;
            return answer;
        }
    }
}