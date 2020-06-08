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
            Guid ownerId = new Guid("00000000-0000-0000-0000-000000000002");
            string body = "BodyNormal";
            _target = Answer.Create(ownerId, body, question, _limits);
            return this;
        }

        public AnswerBuilder SetupAnotherValidAnswer(Question question)
        {
            Guid ownerId = new Guid();
            string body = "BodyNormal";
            _target = Answer.Create(ownerId, body, question, _limits);
            return this;
        }

        public Answer Build() => _target;
    }
}