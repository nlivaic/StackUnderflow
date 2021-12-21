using System;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Limits : ILimits
    {
        public Limits(ILimitsService limitsService)
        {
            var limits = limitsService.Get().Result;
            AnswerEditDeadline = limits.AnswerEditDeadline;
            QuestionEditDeadline = limits.QuestionEditDeadline;
            CommentEditDeadline = limits.CommentEditDeadline;
            VoteEditDeadline = limits.VoteEditDeadline;
            AcceptAnswerDeadline = limits.AcceptAnswerDeadline;
            QuestionBodyMinimumLength = limits.QuestionBodyMinimumLength;
            AnswerBodyMinimumLength = limits.AnswerBodyMinimumLength;
            CommentBodyMinimumLength = limits.CommentBodyMinimumLength;
            TagMinimumCount = limits.TagMinimumCount;
            TagMaximumCount = limits.TagMaximumCount;
            UsernameMinimumLength = limits.UsernameMinimumLength;
            UsernameMaximumLength = limits.UsernameMaximumLength;
            AboutMeMaximumLength = limits.AboutMeMaximumLength;
            UpvotePoints = limits.UpvotePoints;
            DownvotePoints = limits.DownvotePoints;
        }
        private Limits()
        {
        }

        public TimeSpan AnswerEditDeadline { get; private set; }

        public TimeSpan QuestionEditDeadline { get; set; }

        public TimeSpan CommentEditDeadline { get; set; }

        public TimeSpan VoteEditDeadline { get; set; }

        public TimeSpan AcceptAnswerDeadline { get; set; }

        public int QuestionBodyMinimumLength { get; set; }

        public int AnswerBodyMinimumLength { get; set; }

        public int CommentBodyMinimumLength { get; set; }

        public int TagMinimumCount { get; set; }

        public int TagMaximumCount { get; set; }

        public int UsernameMinimumLength { get; set; }

        public int UsernameMaximumLength { get; set; }

        public int AboutMeMaximumLength { get; set; }

        public int UpvotePoints { get; set; }

        public int DownvotePoints { get; set; }

        public static Limits Create() =>
            new ();
    }
}
