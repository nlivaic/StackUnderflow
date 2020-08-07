using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Entities
{
    public class Answer : BaseEntity<Guid>, IVoteable, ICommentable
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public string Body { get; private set; }
        public bool IsAcceptedAnswer { get; private set; }
        public DateTime? AcceptedOn { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Question Question { get; private set; }
        public Guid QuestionId { get; private set; }
        public IEnumerable<Comment> Comments => _commentable.Comments;
        public int VotesSum
        {
            get => _voteable.VotesSum;
            private set => _voteable.VotesSum = value;
        }
        public IEnumerable<Vote> Votes => _voteable.Votes;

        private Voteable _voteable;
        private Commentable _commentable;

        private Answer()
        {
            _commentable = new Commentable();
            _voteable = new Voteable();
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

        public void Edit(User user, string body, ILimits limits)
        {
            if (User.Id != user.Id)
            {
                throw new BusinessException("Question can be edited only by user.");
            }
            if (CreatedOn.Add(limits.AnswerEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"Answer with id '{Id}' cannot be edited since more than '{limits.AnswerEditDeadline.Minutes}' minutes passed.");
            }
            Body = body;
        }

        public void Comment(Comment comment) =>
            _commentable.Comment(comment);

        public void ApplyVote(Vote vote) => _voteable.ApplyVote(vote);

        public void RevokeVote(Vote vote, ILimits limits) => _voteable.RevokeVote(vote, limits);

        public static Answer Create(
            User user,
            string body,
            Question question,
            ILimits limits)
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

        private static void Validate(User user, string body, ILimits limits)
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
    }
}
