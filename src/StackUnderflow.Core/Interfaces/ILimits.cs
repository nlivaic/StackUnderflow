using System;

namespace StackUnderflow.Core.Interfaces
{
    public interface ILimits
    {
        public TimeSpan AnswerEditDeadline { get; }
        public TimeSpan QuestionEditDeadline { get; }
        public TimeSpan CommentEditDeadline { get; }
        public TimeSpan VoteEditDeadline { get; }
        public TimeSpan AcceptAnswerDeadline { get; }
        public int QuestionBodyMinimumLength { get; }
        public int AnswerBodyMinimumLength { get; }
        public int CommentBodyMinimumLength { get; }
        public int TagMinimumCount { get; }
        public int TagMaximumCount { get; }
        public int UsernameMinimumLength { get; }
        public int UsernameMaximumLength { get; }
        public int AboutMeMaximumLength { get; }
        public int UpvotePoints { get; }
        public int DownvotePoints { get; }
    }
}