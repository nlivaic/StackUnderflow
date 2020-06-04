using System;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Services
{
    public class DeadlineService : IDeadlineService
    {
        // @nl: implement caching
        public TimeSpan AnswerEditDeadline => new TimeSpan(0, 10, 0);

        public TimeSpan QuestionEditDeadline => new TimeSpan(0, 10, 0);
    }
}