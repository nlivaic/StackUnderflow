using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Entities
{
    public class Question : BaseEntity<Guid>, IVoteable, ICommentable
    {
        public Guid OwnerId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public bool HasAcceptedAnswer { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public IEnumerable<Answer> Answers => _answers;
        public IEnumerable<Comment> Comments => _commentable.Comments;
        public IEnumerable<QuestionTag> QuestionTags => _questionTags;
        public int VotesSum => _voteable.VotesSum;
        public IEnumerable<Vote> Votes => _voteable.Votes;

        private List<Answer> _answers = new List<Answer>();
        private List<QuestionTag> _questionTags = new List<QuestionTag>();
        private IVoteable _voteable;
        private ICommentable _commentable;

        private Question()
        { }

        public void Edit(Guid ownerId, string title, string body, IEnumerable<Tag> tags, ILimits limits)
        {
            if (OwnerId != ownerId)
            {
                throw new BusinessException("Question can be edited only by owner.");
            }
            if (CreatedOn.Add(limits.QuestionEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"Question with id '{Id}' cannot be edited since more than '{limits.QuestionEditDeadline.Minutes}' minutes passed.");
            }
            Validate(ownerId, title, body, tags, limits);
            Title = title;
            Body = body;
            _questionTags = new List<QuestionTag>(tags.Select(t => new QuestionTag { Question = this, Tag = t }));
        }

        public void Answer(Answer answer)
        {
            if (_answers.Any(a => a.OwnerId == answer.OwnerId))
            {
                throw new BusinessException($"User '{answer.OwnerId}' has already submitted an answer.");
            }
            _answers.Add(answer);
            // @nl: Raise an event!
        }

        public void AcceptAnswer(Answer answer)
        {
            if (_answers.Find(a => a.Id == answer.Id) == null)
            {
                throw new BusinessException($"Answer '{answer.Id}' not associated with question '{this.Id}'.");
            }
            if (HasAcceptedAnswer)
            {
                throw new BusinessException($"Question '{this.Id}' already has an accepted answer.");
            }
            answer.AcceptedAnswer();
            HasAcceptedAnswer = true;
        }

        public void UndoAcceptAnswer(Answer answer, ILimits limits)
        {
            if (_answers.Find(a => a.Id == answer.Id) == null)
            {
                throw new BusinessException($"Answer '{answer.Id}' not associated with question '{this.Id}'.");
            }
            if (answer.AcceptedOn + limits.AcceptAnswerDeadline < DateTime.UtcNow)
            {
                throw new BusinessException($"You cannot undo accepting the answer '{this.Id}' since more than '{limits.AcceptAnswerDeadline.Minutes}' minutes passed.");
            }
            answer.UndoAcceptedAnswer();
            HasAcceptedAnswer = false;
        }

        public void Comment(Comment comment) =>
            _commentable.Comment(comment);

        public void ApplyVote(Vote vote) => _voteable.ApplyVote(vote);

        public void RevokeVote(Vote vote, ILimits limits) => _voteable.RevokeVote(vote, limits);

        public static Question Create(Guid ownerId,
            string title,
            string body,
            IEnumerable<Tag> tags,
            ILimits limits,
            IVoteable voteable,
            ICommentable commentable)
        {
            var question = new Question();
            question.Id = Guid.NewGuid();
            question.OwnerId = ownerId;
            Validate(ownerId, title, body, tags, limits);
            question.Title = title;
            question.Body = body;
            question.HasAcceptedAnswer = false;
            question.CreatedOn = DateTime.UtcNow;
            question._questionTags = new List<QuestionTag>(tags.Select(t => new QuestionTag { Question = question, Tag = t }));
            question._voteable = voteable ?? throw new ArgumentException($"Missing {nameof(IVoteable)} parameter.");
            question._commentable = commentable ?? throw new ArgumentException($"Missing {nameof(ICommentable)} parameter.");
            return question;
        }

        private static void Validate(Guid ownerId, string title, string body, IEnumerable<Tag> tags, ILimits limits)
        {
            if (ownerId == default(Guid))
            {
                throw new BusinessException("Owner id cannot be default Guid.");
            }
            if (body == null || body.Length < limits.QuestionBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.QuestionBodyMinimumLength}' characters.");
            }
            if (string.IsNullOrWhiteSpace(title)) throw new BusinessException("Question must have a title.");
            if (string.IsNullOrWhiteSpace(body)) throw new BusinessException("Question must have a body.");
            var tagCount = tags.Count();
            if (tags == null || tagCount < limits.TagMinimumCount || tagCount > limits.TagMaximumCount) throw new BusinessException("Question must be tagged with at least one and no more than five tags.");
        }
    }
}