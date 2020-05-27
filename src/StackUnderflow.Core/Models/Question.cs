using StackUnderflow.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Models
{
    public class Question : BaseEntity<Guid>
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public bool HasAcceptedAnswer { get; private set; }
        public IEnumerable<Answer> Answers => _answers;
        public IEnumerable<Comment> Comments => _comments;
        public IEnumerable<Tag> Tags => _tags;

        private List<Answer> _answers;
        private List<Comment> _comments;
        private List<Tag> _tags;

        public Question(string title, string body, IEnumerable<Tag> tags, IEnumerable<Comment> comments = null, IEnumerable<Answer> answers = null)
        {
            Title = title ?? throw new ArgumentException("Question must have a title.");
            Body = body ?? throw new ArgumentException("Question must have a body.");
            HasAcceptedAnswer = false;
            _tags = tags == null || tags.Count() == 0
                ? throw new ArgumentException("Question must have a body.")
                : new List<Tag>(tags);
            _comments = comments == null ? new List<Comment>() : new List<Comment>(comments);
            _answers = answers == null ? new List<Answer>() : new List<Answer>(answers);
        }

        public void Edit(string title, string body, IEnumerable<Tag> tags)
        {
            Title = title ?? throw new ArgumentException("Question must have a title.");
            Body = body ?? throw new ArgumentException("Question must have a body.");
            _tags = tags == null || tags.Count() == 0
                ? throw new ArgumentException("Question must have a body.")
                : new List<Tag>(tags);
        }

        public void Answer(Answer answer)
        {
            if (_answers.Any(a => a.Id == answer.Id))
            {
                throw new ArgumentException($"Answer '{answer.Id}' has already been submitted.");
            }
            _answers.Add(answer);
            // @nl: Raise an event!
        }

        public void AcceptAnswer()
        {
            HasAcceptedAnswer = true;
        }
    }
}