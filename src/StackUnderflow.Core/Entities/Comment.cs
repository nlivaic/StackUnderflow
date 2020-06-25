using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace StackUnderflow.Core.Entities
{
    public class Comment : BaseEntity<Guid>, IVoteable
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public string Body { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Question ParentQuestion { get; private set; }
        public Guid ParentQuestionId { get; private set; }
        public Answer ParentAnswer { get; private set; }
        public Guid ParentAnswerId { get; private set; }
        public int OrderNumber { get; private set; }
        public int VotesSum => _voteable.VotesSum;
        public IEnumerable<Vote> Votes => _voteable.Votes;

        private IVoteable _voteable;

        private Comment()
        {
        }

        public void Edit(Guid userId, string body, ILimits limits)
        {
            if (UserId != userId)
            {
                throw new BusinessException("Comment can be edited only by user.");
            }
            if (CreatedOn.Add(limits.CommentEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"Comment with id '{Id}' cannot be edited since more than '{limits.CommentEditDeadline.Minutes}' minutes passed.");
            }
            Validate(userId, body, limits);
            Body = body;
        }

        public void ApplyVote(Vote vote) => _voteable.ApplyVote(vote);

        public void RevokeVote(Vote vote, ILimits limits) => _voteable.RevokeVote(vote, limits);

        public static Comment Create(Guid userId,
            string body,
            int orderNumber,
            ILimits limits,
            IVoteable voteable)
        {
            Validate(userId, body, limits);
            if (orderNumber < 1)
            {
                throw new BusinessException("Order number must be positive.");
            }
            var comment = new Comment();
            comment.Id = Guid.NewGuid();
            comment.UserId = userId;
            comment.Body = body;
            comment.OrderNumber = orderNumber;
            comment.CreatedOn = DateTime.UtcNow;
            comment._voteable = voteable ?? throw new ArgumentException($"Missing {nameof(IVoteable)} parameter.");
            return comment;
        }

        private static void Validate(Guid userId, string body, ILimits limits)
        {
            if (userId == default(Guid))
            {
                throw new BusinessException("User id cannot be default Guid.");
            }
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
        }
    }
}