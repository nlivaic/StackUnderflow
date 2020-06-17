using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Tests.Builders;
using Xunit;

namespace StackUnderflow.Core.Tests
{
    public class VoteableTests
    {
        [Fact]
        public void Voteable_CanApplyVoteToQuestion_Successfully()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();

            // Act
            target.ApplyVote(vote);

            // Assert
            Assert.Equal(1, target.VotesSum);
            Assert.Contains(vote, target.Votes);
        }

        [Fact]
        public void Voteable_CanApplyVoteToAnswer_Successfully()
        {
            // Arrange
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder().SetupValidAnswer(question).Build();
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();

            // Act
            target.ApplyVote(vote);

            // Assert
            Assert.Equal(1, target.VotesSum);
            Assert.Contains(vote, target.Votes);
        }

        [Fact]
        public void Voteable_CanApplyVoteToComment_Successfully()
        {
            // Arrange
            var target = new CommentBuilder().SetupValidComment().Build();
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();

            // Act
            target.ApplyVote(vote);

            // Assert
            Assert.Equal(1, target.VotesSum);
            Assert.Contains(vote, target.Votes);
        }

        [Fact]
        public void Voteable_SameOwnerCannotApplyVoteTwice_Throws()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstVote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();
            var secondVote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();
            target.ApplyVote(firstVote);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.ApplyVote(secondVote));
        }

        [Fact]
        public void Voteable_CanRevokeVoteWithinDeadline_Successfully()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();
            var limits = new LimitsBuilder().Build();
            target.ApplyVote(vote);

            // Act
            target.RevokeVote(vote, limits);

            // Assert
            Assert.Equal(0, target.VotesSum);
        }

        [Fact]
        public void Voteable_CannotRevokeVoteOutsideDeadline_Successfully()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var limits = new LimitsBuilder().Build();
            var firstVote = new VoteBuilder(target)
                .SetupValidUpvote()
                .ByOneOwner()
                .MakeTimeGoBy()
                .Build();
            target.ApplyVote(firstVote);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.RevokeVote(firstVote, limits));
        }

        [Fact]
        public void Voteable_CannotRevokeNonExistingVote_Throws()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var limits = new LimitsBuilder().Build();
            var firstVote = new VoteBuilder(target).SetupValidUpvote().ByOneOwner().Build();
            var secondVote = new VoteBuilder(target).SetupValidUpvote().ByAnotherOwner().Build();
            target.ApplyVote(firstVote);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.RevokeVote(secondVote, limits));
        }
    }
}