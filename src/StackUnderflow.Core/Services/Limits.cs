using System;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Services
{
    public class Limits : ILimits
    {
        // @nl: fetch this from database and implement caching
        public TimeSpan AnswerEditDeadline => new TimeSpan(0, 10, 0);
        public TimeSpan QuestionEditDeadline => new TimeSpan(0, 10, 0);
        public TimeSpan VoteEditDeadline => new TimeSpan(0, 10, 0);
        public int QuestionBodyMinimumLength => 100;
        public int AnswerBodyMinimumLength => 100;
        public int CommentBodyMinimumLength => 30;
    }
}
