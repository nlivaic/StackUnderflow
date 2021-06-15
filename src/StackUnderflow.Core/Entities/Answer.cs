using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Entities
{
    public class Answer : BaseEntity<Guid>, IVoteable, ICommentable, IOwneable
    {
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

        private Voteable _voteable;
        private Commentable _commentable;
        private Owneable _owneable;

        private Answer()
        {
            _commentable = new Commentable();
            _voteable = new Voteable();
            _owneable = new Owneable();
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

        public static Answer Create(
            User user,
            string body,
            Question question,
            BaseLimits limits)
        {
            Validate(user, body, limits);
            var answer = new Answer();
            answer.Id = Guid.NewGuid();
            answer.User = user;
            answer.Body = body;
            answer.IsAcceptedAnswer = false;
            answer.CreatedOn = DateTime.UtcNow;
            return answer;
        }

        private static void Validate(User user, string body, BaseLimits limits)
        {
            if (user.Id == default(Guid))       // @nl: glupo!!!
            {
                throw new BusinessException("User id cannot be default Guid.");
            }
            if (string.IsNullOrWhiteSpace(body) || body.Length < limits.AnswerBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.AnswerBodyMinimumLength}' characters.");
            }
            if (string.IsNullOrWhiteSpace(body)) throw new BusinessException("Question must have a body.");
        }

        public bool CanBeEditedBy(User editingUser) =>
            _owneable.CanBeEditedBy(editingUser);

        public bool IsDeleteable(int votesSum)
        {
            if (IsAcceptedAnswer)
            {
                throw new BusinessException($"Answer with id '{Id}' has been accepted on '{AcceptedOn}'.");
            }
            if (votesSum > 0)
            {
                throw new BusinessException($"Cannot delete answer '{Id}' because associated votes exist.");
            }
            return true;
        }
    }
}
