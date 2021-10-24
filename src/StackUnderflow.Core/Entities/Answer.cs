using System;
using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Answer : BaseEntity<Guid>, IVoteable, ICommentable, IOwneable
    {
        private readonly Voteable _voteable;
        private readonly Commentable _commentable;
        private readonly Owneable _owneable;

        private Answer()
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
        public string Body { get; private set; }
        public bool IsAcceptedAnswer { get; private set; }
        public DateTime? AcceptedOn { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Question Question { get; private set; }
        public Guid QuestionId { get; private set; }
        public IEnumerable<Comment> Comments => _commentable.Comments;
        public IEnumerable<Vote> Votes => _voteable.Votes;

        public static Answer Create(
            User user,
            string body,
            BaseLimits limits)
        {
            Validate(user, body, limits);
            var answer = new Answer
            {
                Id = Guid.NewGuid(),
                User = user,
                Body = body,
                IsAcceptedAnswer = false,
                CreatedOn = DateTime.UtcNow
            };
            return answer;
        }

        public void AcceptedAnswer()
        {
            IsAcceptedAnswer = true;
            AcceptedOn = DateTime.UtcNow;
        }

        public void UndoAcceptedAnswer()
        {
            IsAcceptedAnswer = false;
            AcceptedOn = null;
        }

        public void Edit(User user, string body, BaseLimits limits)
        {
            if (!CanBeEditedBy(user))
            {
                throw new BusinessException("Answer can be edited only by user.");
            }
            if (CreatedOn.Add(limits.AnswerEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"Answer with id '{Id}' cannot be edited since more than '{limits.AnswerEditDeadline.Minutes}' minutes passed.");
            }
            Validate(user, body, limits);
            Body = body;
        }

        public void Comment(Comment comment) =>
            _commentable.Comment(comment);

        public void ApplyVote(Vote vote) => _voteable.ApplyVote(vote);

        public void RevokeVote(Vote vote, BaseLimits limits) => _voteable.RevokeVote(vote, limits);

        public bool CanBeEditedBy(User editingUser) =>
            _owneable.CanBeEditedBy(editingUser);

        public bool IsDeleteable()
        {
            if (IsAcceptedAnswer)
            {
                throw new BusinessException($"Answer with id '{Id}' has been accepted on '{AcceptedOn}'.");
            }
            if (Votes.Any())
            {
                throw new BusinessException($"Cannot delete answer '{Id}' because associated votes exist.");
            }
            if (Comments.SelectMany(c => c.Votes).Any())
            {
                throw new BusinessException($"Cannot delete because associated votes exist on at least one comment.");
            }
            return true;
        }

        private static void Validate(User user, string body, BaseLimits limits)
        {
            // @nl: glupo!!!
            if (user.Id == default(Guid))
            {
                throw new BusinessException("User id cannot be default Guid.");
            }
            if (string.IsNullOrWhiteSpace(body) || body.Length < limits.AnswerBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.AnswerBodyMinimumLength}' characters.");
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new BusinessException("Question must have a body.");
            }
        }
    }
}
