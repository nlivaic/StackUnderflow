using System;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Tests.Builders;
using static StackUnderflow.Core.Entities.Vote;

namespace StackUnderflow.Core.Tests
{
    public class VoteBuilder
    {
        private Vote _target;
        private readonly Question _question;
        private Guid _userId;
        private readonly Answer _answer;
        private readonly Comment _comment;
        private VoteTypeEnum _voteType;
        private BaseLimits _limits = new LimitsBuilder().Build();
        private DateTime? _createdOn;

        public VoteBuilder(Question question)
        {
            _question = question;
        }

        public VoteBuilder(Answer answer)
        {
            _answer = answer;
        }

        public VoteBuilder(Comment comment)
        {
            _comment = comment;
        }

        public VoteBuilder SetupValidUpvote()
        {
            _voteType = VoteTypeEnum.Upvote;
            return this;
        }

        public VoteBuilder SetupValidDownvote()
        {
            _voteType = VoteTypeEnum.Downvote;
            return this;
        }

        public VoteBuilder ByOneUser()
        {
            _userId = new Guid("00000000-0000-0000-0000-000000000001");
            return this;
        }

        public VoteBuilder ByAnotherUser()
        {
            _userId = new Guid("00000000-0000-0000-0000-000000000002");
            return this;
        }

        public VoteBuilder MakeTimeGoBy()
        {
            _createdOn = DateTime.UtcNow.AddMinutes(-1 - _limits.VoteEditDeadline.Minutes);
            return this;
        }

        public Vote Build()
        {
            if (_question != null)
            {
                _target = Vote.CreateVote(_userId, _question, _voteType);
            }
            else if (_answer != null)
            {
                _target = Vote.CreateVote(_userId, _answer, _voteType);
            }
            else
            {
                _target = Vote.CreateVote(_userId, _comment, _voteType);
            }
            _target.SetProperty(
                nameof(_target.CreatedOn),
                _createdOn ?? _target.CreatedOn);
            return _target;
        }
    }
}
