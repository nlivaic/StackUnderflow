using System;
using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Question : BaseEntity<Guid>, IVoteable, ICommentable, IOwneable
    {
        private readonly List<Answer> _answers = new ();
        private readonly Voteable _voteable;
        private readonly Commentable _commentable;
        private readonly Owneable _owneable;
        private List<QuestionTag> _questionTags = new ();

        private Question()
        {
            _commentable = new Commentable();
            _voteable = new Voteable();
            _owneable = new Owneable();
        }

        public Guid UserId
        {
            get => _owneable.UserId;
            private set
            {
                _owneable.UserId = value;
            }
        }
        public User User
        {
            get => _owneable.User;
            private set
            {
                _owneable.User = value;
            }
        }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public bool HasAcceptedAnswer { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public IEnumerable<Answer> Answers => _answers;
        public IEnumerable<Comment> Comments => _commentable.Comments;
        public IEnumerable<QuestionTag> QuestionTags => _questionTags;
        public IEnumerable<Vote> Votes => _voteable.Votes;

        public static Question Create(
            User user,
            string title,
            string body,
            IEnumerable<Tag> tags,
            ILimits limits)
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id
            };
            Validate(user, title, body, tags, limits);
            question.Title = title;
            question.Body = body;
            question.HasAcceptedAnswer = false;
            question.CreatedOn = DateTime.UtcNow;
            question._questionTags = new List<QuestionTag>(tags.Select(t => new QuestionTag { Question = question, Tag = t }));
            return question;
        }

        public void Edit(User user, string title, string body, IEnumerable<Tag> tags, ILimits limits)
        {
            if (!CanBeEditedBy(user))
            {
                throw new BusinessException("Question can be edited only by user.");
            }
            if (CreatedOn.Add(limits.QuestionEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"Question with id '{Id}' cannot be edited since more than '{limits.QuestionEditDeadline.Minutes}' minutes passed.");
            }
            Validate(user, title, body, tags, limits);
            Title = title;
            Body = body;
            _questionTags = new List<QuestionTag>(tags.Select(t => new QuestionTag { Question = this, Tag = t }));
        }

        public void Answer(Answer answer)
        {
            if (_answers.Any(a => a.UserId == answer.User.Id))
            {
                throw new BusinessException($"User '{answer.User.Id}' has already submitted an answer.");
            }
            _answers.Add(answer);

            // @nl: Raise an event!
        }

        public void AcceptAnswer(Answer answer, Guid acceptingUserId)
        {
            if (_answers.Find(a => a.Id == answer.Id) == null)
            {
                throw new BusinessException($"Answer '{answer.Id}' not associated with question '{Id}'.");
            }
            if (HasAcceptedAnswer)
            {
                throw new BusinessException($"Question '{Id}' already has an accepted answer.");
            }
            if (UserId != acceptingUserId)
            {
                throw new BusinessException("Only question owner can accept an answer!");
            }
            answer.AcceptedAnswer();
            HasAcceptedAnswer = true;
        }

        public void UndoAcceptAnswer(Answer answer, ILimits limits)
        {
            if (_answers.Find(a => a.Id == answer.Id) == null)
            {
                throw new BusinessException($"Answer '{answer.Id}' not associated with question '{Id}'.");
            }
            if (answer.AcceptedOn + limits.AcceptAnswerDeadline < DateTime.UtcNow)
            {
                throw new BusinessException($"You cannot undo accepting the answer '{Id}' since more than '{limits.AcceptAnswerDeadline.Minutes}' minutes passed.");
            }
            answer.UndoAcceptedAnswer();
            HasAcceptedAnswer = false;
        }

        public void Comment(Comment comment) => _commentable.Comment(comment);

        public void ApplyVote(Vote vote) => _voteable.ApplyVote(vote);

        public void RevokeVote(Vote vote, ILimits limits) => _voteable.RevokeVote(vote, limits);

        public bool CanBeEditedBy(User editingUser) =>
            _owneable.CanBeEditedBy(editingUser);

        public bool IsDeleteable()
        {
            if (Answers.Any() == true)
            {
                throw new BusinessException($"Cannot delete question '{Id}' because associated answers exist.");
            }
            if (Votes.Any())
            {
                throw new BusinessException($"Cannot delete question '{Id}' because associated votes exist.");
            }
            if (Comments.SelectMany(c => c.Votes).Any())
            {
                throw new BusinessException($"Cannot delete because associated votes exist on at least one comment.");
            }
            return true;
        }

        private static void Validate(User user, string title, string body, IEnumerable<Tag> tags, ILimits limits)
        {
            if (user.Id == default(Guid))
            {
                throw new BusinessException("User id cannot be default Guid.");
            }
            if (body == null || body.Length < limits.QuestionBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.QuestionBodyMinimumLength}' characters.");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BusinessException("Question must have a title.");
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new BusinessException("Question must have a body.");
            }
            var tagCount = tags.Count();
            if (tags == null || tagCount < limits.TagMinimumCount || tagCount > limits.TagMaximumCount)
            {
                throw new BusinessException("Question must be tagged with at least one and no more than five tags.");
            }
        }
    }
}
