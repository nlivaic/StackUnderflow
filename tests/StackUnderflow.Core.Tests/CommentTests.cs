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
        public void Comment_CanBeEditedWithinDeadline_Successfully()
        {
            throw new NotImplementedException("Comment_CanBeEditedWithinDeadline_Successfully");
        }

        [Fact]
        public void Comment_EditedOutsideDeadline_Fails()
        {
            throw new NotImplementedException("Comment_CanBeEditedWithinDeadline_Successfully");
            // Arrange - Build Question, 1 minute after deadline.
            var limits = new LimitsBuilder().Build();
            var target = new QuestionBuilder()
                .SetupValidQuestion()
                .MakeTimeGoBy()
                .Build();

            // Arrange - Edit Data
            string editedTitle = "TitleNormal";
            string editedBody = "BodyNormal";
            int editedTagCount = 5;
            var editedTags = new TagBuilder().Build(editedTagCount);

            // Act
            Assert.Throws<BusinessException>(() =>
               target.Edit(target.OwnerId, editedTitle, editedBody, editedTags, limits));
        }

        // [Fact]
        // public void Comment_EditingWithInvalidData_FailsValidation(string ownerId, int orderNumber, string body)
        // {
        //     throw new NotImplementedException("Comment_EditingWithInvalidData_FailsValidation");
        // }
    }
}
