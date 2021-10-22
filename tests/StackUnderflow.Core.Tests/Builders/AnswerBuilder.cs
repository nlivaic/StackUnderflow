using System;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Tests.Builders
{
    public class AnswerBuilder
    {
        private Answer _target;
        private BaseLimits _limits = new LimitsBuilder().Build();

        public AnswerBuilder SetupValidAnswer(Question question, Guid? userId = null)
        {
            string body = "BodyNormal";
            userId = userId ?? Guid.NewGuid();
            var user = new UserBuilder().BuildUser(userId.Value).Build();
            _target = Answer.Create(user, body, _limits);
            return this;
        }

        public AnswerBuilder SetupAnotherValidAnswer(Question question)
        {
            string body = "BodyNormal";
            var user = new UserBuilder().BuildValidUser().Build();
            _target = Answer.Create(user, body, _limits);
            return this;
        }

        public AnswerBuilder MakeTimeGoBy()
        {
            _target.SetProperty(
                nameof(_target.CreatedOn),
                DateTime.UtcNow.AddMinutes(-1 - _limits.AnswerEditDeadline.Minutes));
            return this;
        }

        public Answer Build() => _target;
    }
}