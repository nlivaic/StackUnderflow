using System;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Tests.Builders;
using Xunit;

namespace StackUnderflow.Core.Tests
{
    public class AnswerTests
    {
        [Fact]
        public void Answer_CanBeCreated_Successfully()
        {
            // Arrange
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var user = new UserBuilder().BuildValidUser().Build();
            string body = "BodyNormal";
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();
            var commentable = new Commentable();

            // Act
            var result = Answer.Create(user, body, limits);

            // Assert
            Assert.NotEqual(default(Guid), result.Id);
            Assert.Equal(user, result.User);
            Assert.Equal(body, result.Body);
            Assert.False(result.IsAcceptedAnswer);
            Assert.Null(result.AcceptedOn);
            Assert.True(DateTime.UtcNow - result.CreatedOn < TimeSpan.FromSeconds(1));
            Assert.Empty(result.Comments);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", "123456789")]
        public void Answer_CreatingWithInvalidData_FailsValidation(string userId, string body)
        {
            // Arrange
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();
            var commentable = new Commentable();
            var user = new UserBuilder().BuildUser(new Guid(userId)).Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
                Answer.Create(user, body, limits));
        }

        [Fact]
        public void Answer_EditingWithValidDataWithinDeadline_Successfully()
        {
            // Arrange - Build Question
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder()
                .SetupValidAnswer(question)
                .Build();
            var userId = target.UserId;

            // Arrange - Edit Data
            string editedBody = "BodyNormal";
            var limits = new LimitsBuilder().Build();

            // Act
            target.Edit(target.User, editedBody, limits);
            var result = target;

            // Assert
            Assert.NotEqual(default(Guid), result.Id);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(editedBody, result.Body);
            Assert.Empty(result.Comments);
        }

        [Fact]
        public void Answer_EditingOutsideDeadline_Fails()
        {
            // Arrange - Build Answer, 1 minute after deadline.
            var limits = new LimitsBuilder().Build();
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder()
                .SetupValidAnswer(question)
                .MakeTimeGoBy()
                .Build();

            // Arrange - Edit Data
            string editedBody = "BodyNormal";

            // Act
            Assert.Throws<BusinessException>(() =>
               target.Edit(target.User, editedBody, limits));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", "123456789")]
        [InlineData("00000000-0000-0000-0000-000000000001", "BodyNormal")]
        public void Answer_EditingWithInvalidData_FailsValidation(string userId, string body)
        {
            // Arrange - Build Answer
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder().SetupValidAnswer(question).Build();
            var limits = new LimitsBuilder().Build();
            var user = new UserBuilder().BuildUser(new Guid(userId)).Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
               target.Edit(user, body, limits));
        }

        [Fact]
        public void Answer_AcceptedAnswer_Successfully()
        {
            // Arrange
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder().SetupValidAnswer(question).Build();
            var originalId = target.Id;
            var originalUserId = target.UserId;
            var originalBody = target.Body;
            var originalCreatedOn = target.CreatedOn;
            var originalQuestion = target.Question;

            // Act
            target.AcceptedAnswer();

            // Assert
            Assert.Equal(originalId, target.Id);
            Assert.Equal(originalUserId, target.UserId);
            Assert.Equal(originalBody, target.Body);
            Assert.Equal(originalCreatedOn, target.CreatedOn);
            Assert.True(target.IsAcceptedAnswer);
            Assert.True(DateTime.UtcNow - target.AcceptedOn < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Answer_UndoAcceptedAnswer_Successfully()
        {
            // Arrange
            var question = new QuestionBuilder().SetupValidQuestion().Build();
            var target = new AnswerBuilder().SetupValidAnswer(question).Build();
            var originalId = target.Id;
            var originalUserId = target.UserId;
            var originalBody = target.Body;
            var originalCreatedOn = target.CreatedOn;
            var originalQuestion = target.Question;

            // Act
            target.AcceptedAnswer();
            target.UndoAcceptedAnswer();

            // Assert
            Assert.Equal(originalId, target.Id);
            Assert.Equal(originalUserId, target.UserId);
            Assert.Equal(originalBody, target.Body);
            Assert.Equal(originalCreatedOn, target.CreatedOn);
            Assert.False(target.IsAcceptedAnswer);
            Assert.Null(target.AcceptedOn);
        }
    }
}
