using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Core.Entities
{
    public class Comment : BaseEntity<Guid>, IVoteable, IOwneable
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
        public DateTime CreatedOn { get; private set; }
        public Question ParentQuestion { get; private set; }
        public Guid? ParentQuestionId { get; private set; }
        public Answer ParentAnswer { get; private set; }
        public Guid? ParentAnswerId { get; private set; }
        public int OrderNumber { get; private set; }
        public IEnumerable<Vote> Votes => _voteable.Votes;

        private Voteable _voteable;
        private Owneable _owneable;

        private Comment()
        {
            _voteable = new Voteable();
            _owneable = new Owneable();
        }

        public void Edit(User user, string body, BaseLimits limits)
        {
            if (!CanBeEditedBy(user))
            {
                throw new BusinessException("Comment can be edited only by user.");
            }
            if (CreatedOn.Add(limits.CommentEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"Comment with id '{Id}' cannot be edited since more than '{limits.CommentEditDeadline.Minutes}' minutes passed.");
            }
            Validate(user, body, limits);
            Body = body;
        }

        public void ApplyVote(Vote vote) => _voteable.ApplyVote(vote);

        public void RevokeVote(Vote vote, BaseLimits limits) => _voteable.RevokeVote(vote, limits);

        public static Comment Create(User user,
            string body,
            int orderNumber,
            BaseLimits limits)
        {
            Validate(user, body, limits);
            if (orderNumber < 1)
            {
                throw new BusinessException("Order number must be positive.");
            }
            var comment = new Comment();
            comment.Id = Guid.NewGuid();
            comment.User = user;
            comment.Body = body;
            comment.OrderNumber = orderNumber;
            comment.CreatedOn = DateTime.UtcNow;
            return comment;
        }

        private static void Validate(User user, string body, BaseLimits limits)
        {
            if (user.Id == default(Guid))
            {
                throw new BusinessException("User id cannot be default Guid.");     // @nl: ovo je glupo ovdje kontrolirati, to treba User kontrolirati. Cijeli q/a/c ima ovu validaciju.
            }
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
        }

        public bool CanBeEditedBy(User editingUser) =>
            _owneable.CanBeEditedBy(editingUser);

        public bool IsDeleteable(int votesSum, User deletingUser)
        {
            if (UserId != deletingUser.Id)
            {
                throw new BusinessException($"Only comment owner can delete a comment.");
            }
            if (votesSum > 0)
            {
                throw new BusinessException($"Cannot delete comment '{Id}' because associated votes exist.");
            }
            return true;
        }

        public bool IsDeleteable()
        {
            if (Votes.Any())
            {
                throw new BusinessException($"Cannot delete comment '{Id}' because associated votes exist.");
            }
            return true;
        }
    }
}
