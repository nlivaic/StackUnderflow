using System;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Tests.Builders;
using Xunit;

namespace StackUnderflow.Core.Tests
{
    public class CommentTests
    {
        [Fact]
        public void Comment_CanGetCreatedWithValidData_Successfully()
        {
            // Arrange
            Guid ownerId = new Guid("00000000-0000-0000-0000-000000000001");
            var body = "BodyNormal";
            var orderNumber = 1;
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();

            // Act
            var result = Comment.Create(ownerId, body, orderNumber, limits, voteable);

            // Assert
            Assert.Equal(ownerId, result.OwnerId);
            Assert.Equal(body, result.Body);
            Assert.Equal(orderNumber, result.OrderNumber);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", 1, "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "123456789")]
        [InlineData("00000000-0000-0000-0000-000000000000", -1, "BodyNormal")]
        public void Comment_CreatingWithInvalidData_FailsValidation(string ownerId, int orderNumber, string body)
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();

            // Act, Assert
            Assert.Throws<BusinessException>(() => Comment.Create(new Guid(ownerId), body, orderNumber, limits, voteable));
        }

        [Fact]
        public void Comment_EditingWithValidDataWithinDeadline_Successfully()
        {
            // Arrange
            var target = new CommentBuilder().SetupValidComment().Build();
            var originalId = target.Id;
            var originalOwnerId = target.OwnerId;
            var originalCreatedOn = target.CreatedOn;
            var originalParentQuestion = target.ParentQuestion;
            var originalParentAnswer = target.ParentAnswer;

            var ownerId = new Guid("00000000-0000-0000-0000-000000000002");
            var newBody = "NewBodyNormal";
            var limits = new LimitsBuilder().Build();

            // Act
            target.Edit(ownerId, newBody, limits);

            // Assert
            Assert.Equal(originalId, target.Id);
            Assert.Equal(originalOwnerId, target.OwnerId);
            Assert.Equal(originalCreatedOn, target.CreatedOn);
            Assert.Equal(originalParentQuestion, target.ParentQuestion);
            Assert.Equal(originalParentAnswer, target.ParentAnswer);
            Assert.Equal(newBody, target.Body);
        }

        [Fact]
        public void Comment_EditingOutsideDeadline_Fails()
        {
            // Arrange - Build Question, 1 minute after deadline.
            var limits = new LimitsBuilder().Build();
            var target = new CommentBuilder()
                .SetupValidComment()
                .MakeTimeGoBy()
                .Build();

            // Arrange - Edit Data
            string editedBody = "BodyNormal";

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
               target.Edit(target.OwnerId, editedBody, limits));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", "123456789")]
        public void Comment_EditingWithInvalidData_FailsValidation(string ownerId, string body)
        {
            // Arrange
            var target = new CommentBuilder().SetupValidComment().Build();
            var limits = new LimitsBuilder().Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
               target.Edit(new Guid(ownerId), body, limits));
        }

        [Fact]
        public void Comment_CreatingWithoutVoteable_Fails()
        {
            // Arrange
            Guid ownerId = new Guid("00000000-0000-0000-0000-000000000001");
            string body = "BodyNormal";
            var limits = new LimitsBuilder().Build();
            var orderNumber = 1;
            IVoteable voteable = null;
            var commentable = new Commentable();

            // Act, Assert
            Assert.Throws<ArgumentException>(() =>
                Comment.Create(ownerId, body, orderNumber, limits, voteable));
        }
    }
}
