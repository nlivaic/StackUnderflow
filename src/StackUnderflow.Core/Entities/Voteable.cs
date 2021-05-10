using System;
using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using static StackUnderflow.Core.Entities.Vote;

namespace StackUnderflow.Core.Entities
{
    public class Voteable : IVoteable
    {
        public IEnumerable<Vote> Votes => _votes;
        private List<Vote> _votes = new List<Vote>();

        public void ApplyVote(Vote vote)
        {
            var targetId = vote.QuestionId ?? vote.AnswerId ?? vote.CommentId.Value;
            if (_votes.SingleOrDefault(v => v.UserId == vote.UserId) != null)
            {
                throw new BusinessException($"User '{vote.UserId}' has already voted on {Target(vote)} '{TargetId(vote)}'.");
            }
            _votes.Add(vote);
            // @nl: Tell (q/a/c) target owner that they received an upvote/downvote (use inbox).
            // @nl: initiate point recalculation for (q/a/c) target owner.
        }

        public void RevokeVote(Vote vote, BaseLimits limits)
        {
            if (_votes.SingleOrDefault(v => v.UserId == vote.UserId) == null)
            {
                throw new BusinessException($"Vote does not exist on {vote.Target.Name} '{vote.TargetId}'.");
            }
            if (vote.CreatedOn.Add(limits.VoteEditDeadline) < DateTime.UtcNow)
            {
                throw new BusinessException($"{vote.Target.Name} with id '{vote.TargetId}' cannot be edited since more than '{limits.AnswerEditDeadline.Minutes}' minutes passed.");
            }
            // @nl: Tell (q/a/c) target owner that they received an upvote/downvote (use inbox).
            // @nl: initiate point recalculation for (q/a/c) target owner.
        }
    }
}
