using StackUnderflow.Common.Base;
using StackUnderflow.Core.Interfaces;
using System;

namespace StackUnderflow.Core.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Body { get; set; }
        public Question ParentQuestion { get; set; }
        public Guid ParentQuestionId { get; set; }
        public Answer ParentAnswer { get; set; }
        public Guid ParentAnswerId { get; set; }

        public void Edit(Guid ownerId, string body, ILimits limits)
        {
            if (OwnerId != ownerId)
            {
                throw new ArgumentException("Comment can be edited only by owner.");
            }
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new ArgumentException($"Answer body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
            Body = body;
        }

        public static Comment Create(Guid ownerId, string body, ILimits limits)
        {
            var comment = new Comment();
            comment.Id = Guid.NewGuid();
            comment.OwnerId = ownerId;
            if (body.Length < limits.CommentBodyMinimumLength)
            {
                throw new ArgumentException($"Comment body must be at least '{limits.CommentBodyMinimumLength}' characters.");
            }
            comment.Body = body;
            return comment;
        }
    }
}