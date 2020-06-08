using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using System;

namespace StackUnderflow.Core.Entities
{
    public class Comment : BaseVoteable
    {
        public Guid OwnerId { get; private set; }
        public string Body { get; private set; }
        public Question ParentQuestion { get; private set; }
        public Guid ParentQuestionId { get; private set; }
        public Answer ParentAnswer { get; private set; }
        public Guid ParentAnswerId { get; private set; }
        public int OrderNumber { get; private set; }

        public void Edit(Guid ownerId, string body, ILimits limits)
        {
            if (OwnerId != ownerId)
            {
                throw new BusinessException("Comment can be edited only by owner.");
            }
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
            Body = body;
        }

        public static Comment Create(Guid ownerId, string body, int orderNumber, ILimits limits)
        {
            var comment = new Comment();
            comment.Id = Guid.NewGuid();
            comment.OwnerId = ownerId;
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new BusinessException($"Comment body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
            comment.Body = body;
            comment.OrderNumber = orderNumber;
            return comment;
        }
    }
}