using System;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Tests.Builders
{
    public class CommentBuilder
    {
        private Comment _target;
        private ILimits _limits = new LimitsBuilder().Build();

        public CommentBuilder SetupValidComment(int orderNumber = 1)
        {
            var user = new UserBuilder().BuildUser(new Guid("00000000-0000-0000-0000-000000000002")).Build();
            return SetupValidComment(user, orderNumber);
        }

        public CommentBuilder SetupValidComment(User user, int orderNumber = 1)
        {
            string body = "BodyNormal";
            _target = Comment.Create(user, body, orderNumber, _limits);
            return this;
        }

        public CommentBuilder MakeTimeGoBy()
        {
            _target.SetProperty(
                nameof(_target.CreatedOn),
                DateTime.UtcNow.AddMinutes(-1 - _limits.CommentEditDeadline.Minutes));
            return this;
        }

        public Comment Build() => _target;
    }
}