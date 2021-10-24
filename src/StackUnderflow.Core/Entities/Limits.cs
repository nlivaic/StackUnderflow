using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Limits : BaseLimits
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

        public static Limits Create() =>
            new ();
    }
}
