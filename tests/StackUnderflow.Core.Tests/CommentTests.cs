using System;
using StackUnderflow.Common.Exceptions;
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
            var user = new UserBuilder().BuildValidUser().Build();
            var body = "BodyNormal";
            var orderNumber = 1;
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();

            // Act
            var result = Comment.Create(user, body, orderNumber, limits);

            // Assert
            Assert.Equal(user, result.User);
            Assert.Equal(body, result.Body);
            Assert.Equal(orderNumber, result.OrderNumber);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", 1, "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "123456789")]
        [InlineData("00000000-0000-0000-0000-000000000000", -1, "BodyNormal")]
        public void Comment_CreatingWithInvalidData_FailsValidation(string userId, int orderNumber, string body)
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();
            var user = new UserBuilder().BuildUser(new Guid(userId)).Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() => Comment.Create(user, body, orderNumber, limits));
        }

        [Fact]
        public void Comment_EditingWithValidDataWithinDeadline_Successfully()
        {
            // Arrange
            var target = new CommentBuilder().SetupValidComment().Build();
            var originalId = target.Id;
            var originalUserId = target.UserId;
            var originalCreatedOn = target.CreatedOn;
            var originalParentQuestion = target.ParentQuestion;
            var originalParentAnswer = target.ParentAnswer;

            var userId = new Guid("00000000-0000-0000-0000-000000000002");
            var newBody = "NewBodyNormal";
            var limits = new LimitsBuilder().Build();

            // Act
            target.Edit(target.User, newBody, limits);

            // Assert
            Assert.Equal(originalId, target.Id);
            Assert.Equal(originalUserId, target.UserId);
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
               target.Edit(target.User, editedBody, limits));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", "123456789")]
        public void Comment_EditingWithInvalidData_FailsValidation(string userId, string body)
        {
            // Arrange
            var target = new CommentBuilder().SetupValidComment().Build();
            var limits = new LimitsBuilder().Build();
            var user = new UserBuilder().BuildUser(new Guid(userId)).Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
               target.Edit(user, body, limits));
        }
    }
}
