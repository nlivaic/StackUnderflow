using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Comment : BaseEntity<Guid>, IVoteable, IOwneable
    {
        private readonly Voteable _voteable;
        private readonly Owneable _owneable;

        private Comment()
        {
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
        public DateTime CreatedOn { get; private set; }
        public Question ParentQuestion { get; private set; }
        public Guid? ParentQuestionId { get; private set; }
        public Answer ParentAnswer { get; private set; }
        public Guid? ParentAnswerId { get; private set; }
        public int OrderNumber { get; private set; }
        public IEnumerable<Vote> Votes => _voteable.Votes;

        public static Comment Create(
            User user,
            string body,
            int orderNumber,
            ILimits limits)
        {
            Validate(user, body, limits);
            if (orderNumber < 1)
            {
                throw new BusinessException("Order number must be positive.");
            }
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                User = user,
                UserId = user.Id,
                Body = body,
                OrderNumber = orderNumber,
                CreatedOn = DateTime.UtcNow
            };
            return comment;
        }

        public void Edit(User user, string body, ILimits limits)
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

        public void RevokeVote(Vote vote, ILimits limits) => _voteable.RevokeVote(vote, limits);

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

        private static void Validate(User user, string body, ILimits limits)
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

        public class MapperProfile : Profile
        {
            public MapperProfile()
            {
                CreateMap<Comment, Comment>();
            }
        }
    }
}
