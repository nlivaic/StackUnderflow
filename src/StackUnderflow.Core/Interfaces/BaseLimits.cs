using System;

namespace StackUnderflow.Core.Interfaces
{
    public abstract class BaseLimits
    {
        public TimeSpan AnswerEditDeadline { get; protected set; }
        public TimeSpan QuestionEditDeadline { get; protected set; }
        public TimeSpan CommentEditDeadline { get; protected set; }
        public TimeSpan VoteEditDeadline { get; protected set; }
        public TimeSpan AcceptAnswerDeadline { get; protected set; }
        public int QuestionBodyMinimumLength { get; protected set; }
        public int AnswerBodyMinimumLength { get; protected set; }
        public int CommentBodyMinimumLength { get; protected set; }
        public int TagMinimumCount { get; protected set; }
        public int TagMaximumCount { get; protected set; }
        public int UsernameMinimumLength { get; protected set; }
        public int UsernameMaximumLength { get; protected set; }
        public int AboutMeMaximumLength { get; protected set; }
        public int UpvotePoints { get; protected set; }
        public int DownvotePoints { get; protected set; }
    }
}