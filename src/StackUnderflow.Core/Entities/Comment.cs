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
            Validate(ownerId, body, limits);
            Body = body;
        }

        public static Comment Create(Guid ownerId, string body, int orderNumber, ILimits limits)
        {
            Validate(ownerId, body, limits);

            if (orderNumber < 1)
            {
                throw new BusinessException("Order number must be positive.");
            }
            var comment = new Comment();
            comment.Id = Guid.NewGuid();
            comment.OwnerId = ownerId;
            comment.Body = body;
            comment.OrderNumber = orderNumber;
            return comment;
        }

        private static void Validate(Guid ownerId, string body, ILimits limits)
        {
            if (ownerId == default(Guid))
            {
                throw new BusinessException("Owner id cannot be default Guid.");
            }
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new BusinessException($"Answer body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
        }
    }
}