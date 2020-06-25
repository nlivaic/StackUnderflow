using System;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Tests.Builders
{
    public class AnswerBuilder
    {
        private Answer _target;
        private ILimits _limits = new LimitsBuilder().Build();

        public AnswerBuilder SetupValidAnswer(Question question)
        {
            Guid userId = new Guid("00000000-0000-0000-0000-000000000002");
            string body = "BodyNormal";
            _target = Answer.Create(userId, body, question, _limits, new Voteable(), new Commentable());
            return this;
        }

        public AnswerBuilder SetupAnotherValidAnswer(Question question)
        {
            Guid userId = Guid.NewGuid();
            string body = "BodyNormal";
            _target = Answer.Create(userId, body, question, _limits, new Voteable(), new Commentable());
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