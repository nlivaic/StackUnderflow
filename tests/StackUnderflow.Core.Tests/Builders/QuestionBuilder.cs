using System;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Tests.Builders
{
    public class QuestionBuilder
    {
        private Question _target;
        private ILimits _limits = new LimitsBuilder().Build();

        public QuestionBuilder SetupValidQuestion()
        {
            var user = new UserBuilder().BuildValidUser().Build();
            int tagCount = 3;
            string title = "TitleNormal";
            string body = "BodyNormal";
            var tags = new TagBuilder().Build(tagCount);
            var voteable = new Voteable();
            var commentable = new Commentable();
            _target = Question.Create(user, title, body, tags, _limits);
            return this;
        }

        public QuestionBuilder MakeTimeGoBy()
        {
            _target.SetProperty(
                nameof(_target.CreatedOn),
                DateTime.UtcNow.AddMinutes(-1 - _limits.QuestionEditDeadline.Minutes));
            return this;
        }

        public Question Build() => _target;
    }
}