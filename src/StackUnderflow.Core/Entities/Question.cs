using StackUnderflow.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Entities
{
    public class Question : BaseEntity<Guid>
    {
        public Guid OwnerId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public bool HasAcceptedAnswer { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IEnumerable<Answer> Answers => _answers;
        public IEnumerable<Comment> Comments => _comments;
        public IEnumerable<Tag> Tags => _tags;

        private List<Answer> _answers;
        private List<Comment> _comments;
        private List<Tag> _tags;

        public Question(Guid id)
        {
            Id = id;
        }

        public void Edit(string title, string body, IEnumerable<Tag> tags)
        {
            Title = title ?? throw new ArgumentException("Question must have a title.");
            Body = body ?? throw new ArgumentException("Question must have a body.");
            _tags = tags == null || tags.Count() == 0
                ? throw new ArgumentException("Question must have a body.")
                : new List<Tag>(tags);
        }

        // @nl: ovo nema smisla ako idemo u anemic model.
        // public void Answer(Answer answer)
        // {
        //     if (_answers.Any(a => a.OwnerId == answer.OwnerId))
        //     {
        //         throw new ArgumentException($"User '{answer.OwnerId}' has already submitted an answer.");
        //     }
        //     _answers.Add(answer);
        //     // @nl: Raise an event!
        // }

        public void AcceptAnswer()
        {
            HasAcceptedAnswer = true;
        }

        public static Question Create(Guid ownerId, string title, string body, IEnumerable<Tag> tags)
        {
            var question = new Question(Guid.NewGuid());
            question.Title = title ?? throw new ArgumentException("Question must have a title.");
            question.Body = body ?? throw new ArgumentException("Question must have a body.");
            question.HasAcceptedAnswer = false;
            question.CreatedAt = DateTime.UtcNow;
            question._tags = tags == null || tags.Count() == 0
                ? throw new ArgumentException("Question must have a body.")
                : new List<Tag>(tags);
            question._comments = new List<Comment>();
            question._answers = new List<Answer>();
            return question;
        }
    }
}