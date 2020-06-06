using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Common.Base;
using static StackUnderflow.Core.Entities.Vote;

namespace StackUnderflow.Core.Entities
{
    public abstract class BaseVoteable : BaseEntity<Guid>
    {
        public int VotesSum { get; private set; } = 0;
        public IEnumerable<Vote> Votes => _votes;
        private List<Vote> _votes = new List<Vote>();

        public void ApplyVote(Vote vote)
        {
            var targetId = vote.QuestionId ?? vote.AnswerId ?? vote.CommentId.Value;
            _votes.Add(vote);
            AssignVote(vote);
            // @nl: Tell (q/a/c) target owner that they received an upvote/downvote (use inbox).
            // @nl: initiate point recalculation for (q/a/c) target owner.
        }

        public void RevokeVote(Vote vote)
        {
            switch (vote.VoteType)
            {
                case VoteTypeEnum.Upvote:
                    VotesSum--;
                    break;
                case VoteTypeEnum.Downvote:
                    VotesSum++;
                    break;
            }

            // @nl: Tell (q/a/c) target owner that they received an upvote/downvote (use inbox).
            // @nl: initiate point recalculation for (q/a/c) target owner.
        }

        private void AssignVote(Vote vote)
        {
            switch (vote.VoteType)
            {
                case VoteTypeEnum.Upvote:
                    VotesSum++;
                    break;
                case VoteTypeEnum.Downvote:
                    VotesSum--;
                    break;
            }
        }
    }
}
