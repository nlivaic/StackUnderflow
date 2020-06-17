using System;

namespace StackUnderflow.Core.Interfaces
{
    public interface ILimits
    {
        TimeSpan QuestionEditDeadline { get; }
        TimeSpan AnswerEditDeadline { get; }
        TimeSpan CommentEditDeadline { get; }
        TimeSpan VoteEditDeadline { get; }
        int QuestionBodyMinimumLength { get; }
        int AnswerBodyMinimumLength { get; }
        int CommentBodyMinimumLength { get; }
        int TagMinimumCount { get; }
        int TagMaximumCount { get; }
    }
}