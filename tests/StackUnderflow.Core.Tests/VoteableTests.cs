using StackUnderflow.Common.Exceptions;
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
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneUser().Build();

            // Act
            target.ApplyVote(vote);

            // Assert
            Assert.Contains(vote, target.Votes);
        }

        [Fact]
        public void Voteable_CanApplyVoteToAnswer_Successfully()
        {
            // Arrange
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder().SetupValidAnswer(question).Build();
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneUser().Build();

            // Act
            target.ApplyVote(vote);

            // Assert
            Assert.Contains(vote, target.Votes);
        }

        [Fact]
        public void Voteable_CanApplyVoteToComment_Successfully()
        {
            // Arrange
            var target = new CommentBuilder().SetupValidComment().Build();
            var vote = new VoteBuilder(target).SetupValidUpvote().ByOneUser().Build();

            // Act
            target.ApplyVote(vote);

            // Assert
            Assert.Contains(vote, target.Votes);
        }

        [Fact]
        public void Voteable_SameUserCannotApplyVoteTwice_Throws()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstVote = new VoteBuilder(target).SetupValidUpvote().ByOneUser().Build();
            var secondVote = new VoteBuilder(target).SetupValidUpvote().ByOneUser().Build();
            target.ApplyVote(firstVote);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.ApplyVote(secondVote));
        }

        [Fact]
        public void Voteable_CannotRevokeVoteOutsideDeadline_Successfully()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var limits = new LimitsBuilder().Build();
            var firstVote = new VoteBuilder(target)
                .SetupValidUpvote()
                .ByOneUser()
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
            var firstVote = new VoteBuilder(target).SetupValidUpvote().ByOneUser().Build();
            var secondVote = new VoteBuilder(target).SetupValidUpvote().ByAnotherUser().Build();
            target.ApplyVote(firstVote);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.RevokeVote(secondVote, limits));
        }
    }
}