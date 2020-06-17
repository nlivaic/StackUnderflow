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
            Guid ownerId = new Guid("00000000-0000-0000-0000-000000000002");
            string body = "BodyNormal";
            var voteable = new Voteable();
            _target = Comment.Create(ownerId, body, orderNumber, _limits, voteable);
            return this;
        }

        public Comment Build() => _target;
    }
}