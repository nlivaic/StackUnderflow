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
        public void Comment_CanGetCreated_Successfully()
        {
            // Arrange
            Guid ownerId = new Guid("00000000-0000-0000-0000-000000000001");
            var body = "BodyNormal";
            var orderNumber = 1;
            var limits = new LimitsBuilder().Build();

            // Act
            var result = Comment.Create(ownerId, body, orderNumber, limits);

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

            // Act, Assert
            Assert.Throws<BusinessException>(() => Comment.Create(new Guid(ownerId), body, orderNumber, limits));
        }
    }
}