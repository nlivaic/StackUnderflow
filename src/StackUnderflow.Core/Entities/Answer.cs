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
        public int VotesSum => _voteable.VotesSum;
        public IEnumerable<Vote> Votes => _voteable.Votes;

        private IVoteable _voteable;
        private ICommentable _commentable;

        // public Answer(string body, Guid questionId, IEnumerable<Comment> comments)
        // {
        //     Body = body ?? throw new ArgumentException("Answer must have a body.");
        //     QuestionId = questionId == Guid.Empty
        //         ? throw new ArgumentException("Answer must relate to a question.")
        //         : questionId;
        //     _comments = comments == null || comments.Count() == 0
        //         ? new List<Comment>()
        //         : new List<Comment>(comments);
        //     CreatedOn = DateTime.UtcNow;
        // }

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

        public void Edit(Guid userId, string body, ILimits limits)
        {
            if (UserId != userId)
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
            Guid userId,
            string body,
            Question question,
            ILimits limits,
            IVoteable voteable,
            ICommentable commentable)
        {
            Validate(userId, body, limits);
            var answer = new Answer();
            answer.Id = Guid.NewGuid();
            answer.UserId = userId;
            answer.Body = body;
            answer.IsAcceptedAnswer = false;
            answer.CreatedOn = DateTime.UtcNow;
            answer._voteable = voteable ?? throw new ArgumentException($"Missing {nameof(IVoteable)} parameter."); ;
            answer._commentable = commentable ?? throw new ArgumentException($"Missing {nameof(ICommentable)} parameter.");
            return answer;
        }

        private static void Validate(Guid userId, string body, ILimits limits)
        {
            if (userId == default(Guid))
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
