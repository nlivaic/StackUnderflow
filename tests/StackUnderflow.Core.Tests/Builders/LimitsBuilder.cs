using Moq;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Tests.Builders
{
    public class LimitsBuilder
    {
        private Mock<ILimits> _target;

        public LimitsBuilder()
        {
            _target = new Mock<ILimits>();
        }

        public ILimits Build()
        {
            _target.Setup(m => m.QuestionEditDeadline).Returns(new System.TimeSpan(0, 10, 0));
            _target.Setup(m => m.AnswerEditDeadline).Returns(new System.TimeSpan(0, 10, 0));
            _target.Setup(m => m.CommentEditDeadline).Returns(new System.TimeSpan(0, 10, 0));
            _target.Setup(m => m.VoteEditDeadline).Returns(new System.TimeSpan(0, 10, 0));
            _target.Setup(m => m.AcceptAnswerDeadline).Returns(new System.TimeSpan(0, 10, 0));
            _target.Setup(m => m.QuestionBodyMinimumLength).Returns(10);
            _target.Setup(m => m.AnswerBodyMinimumLength).Returns(10);
            _target.Setup(m => m.CommentBodyMinimumLength).Returns(10);
            _target.Setup(m => m.TagMinimumCount).Returns(2);
            _target.Setup(m => m.TagMaximumCount).Returns(5);

            return _target.Object;
        }

    }
}