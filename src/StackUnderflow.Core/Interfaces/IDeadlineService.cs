using System;

namespace StackUnderflow.Core.Interfaces
{
    public interface IDeadlineService
    {
        TimeSpan QuestionEditDeadline { get; }
        TimeSpan AnswerEditDeadline { get; }
    }
}