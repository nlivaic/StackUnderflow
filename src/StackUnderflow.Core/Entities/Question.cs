using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Entities
{
    public class Question : BaseVoteable
    {
        public Guid OwnerId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public bool HasAcceptedAnswer { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public IEnumerable<Answer> Answers => _answers;
        public IEnumerable<Comment> Comments => _comments;
        public IEnumerable<Tag> Tags => _tags;

        private List<Answer> _answers;
        private List<Comment> _comments;
        private List<Tag> _tags;

        private Question(Guid id)
        {
            Id = id;
        }

        public void Edit(Guid ownerId, string title, string body, IEnumerable<Tag> tags, ILimits limits)
        {
            if (OwnerId != ownerId)
            {
                throw new ArgumentException("Question can be edited only by owner.");
            }
            if (CreatedOn.Add(limits.QuestionEditDeadline) > DateTime.UtcNow)
            {
                throw new ArgumentException($"Question with id '{Id}' cannot be edited since more than '{limits.QuestionEditDeadline.Minutes}' minutes passed.");
            }
            Title = title ?? throw new ArgumentException("Question must have a title.");
            Body = body ?? throw new ArgumentException("Question must have a body.");
            _tags = tags == null || tags.Count() == 0
                ? throw new ArgumentException("Question must have a body.")
                : new List<Tag>(tags);
        }

        public void Answer(Answer answer)
        {
            if (_answers.Any(a => a.OwnerId == answer.OwnerId))
            {
                throw new ArgumentException($"User '{answer.OwnerId}' has already submitted an answer.");
            }
            _answers.Add(answer);
            // @nl: Raise an event!
        }

        public void AcceptAnswer()
        {
            HasAcceptedAnswer = true;
        }

        public void Comment(Comment comment)
        {
            _comments.Add(comment);
        }

        public static Question Create(Guid ownerId, string title, string body, IEnumerable<Tag> tags, ILimits limits)
        {
            var question = new Question(Guid.NewGuid());
            if (body.Length < limits.QuestionBodyMinimumLength)
            {
                throw new ArgumentException($"Answer body must be at least '{limits.QuestionBodyMinimumLength}' characters.");
            }
            question.Title = title ?? throw new ArgumentException("Question must have a title.");
            question.Body = body ?? throw new ArgumentException("Question must have a body.");
            question.HasAcceptedAnswer = false;
            question.CreatedOn = DateTime.UtcNow;
            var tagCount = tags.Count();
            question._tags = tags == null || tagCount < 1 || tagCount > 5
                ? throw new ArgumentException("Question must be tagged with at least one and no more than five tags.")
                : new List<Tag>(tags);
            question._comments = new List<Comment>();
            question._answers = new List<Answer>();
            return question;
        }
    }
}