using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Limits : BaseLimits
    {
        private Limits()
        {
        }

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

        public static Limits Create() =>
            new Limits();
    }
}
