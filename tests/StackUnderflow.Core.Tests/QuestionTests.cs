using System;
using System.Linq;
using FizzWare.NBuilder;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Tests.Builders;
using Xunit;

namespace StackUnderflow.Core.Tests
{
    public class QuestionTests
    {
        [Fact]
        public void Question_CanBeCreated_Successfully()
        {
            // Arrange
            var user = new UserBuilder().BuildValidUser().Build();
            int tagCount = 3;
            string title = "TitleNormal";
            string body = "BodyNormal";
            var tags = new TagBuilder().Build(tagCount);
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();
            var commentable = new Commentable();

            // Act
            var result = Question.Create(user, title, body, tags, limits);

            // Assert
            Assert.NotEqual(default(Guid), result.Id);
            Assert.Equal(user, result.User);
            Assert.Equal(title, result.Title);
            Assert.Equal(body, result.Body);
            Assert.False(result.HasAcceptedAnswer);
            Assert.True(DateTime.UtcNow - result.CreatedOn < TimeSpan.FromSeconds(1));
            Assert.Empty(result.Answers);
            Assert.Empty(result.Comments);
            Assert.Equal(3, result.QuestionTags.Count());
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", 3, "TitleNormal", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "  ", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "  ", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "123456789", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "", "123456789")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "TitleNormal", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", 6, "TitleNormal", "BodyNormal")]
        public void Question_CreatingWithInvalidData_FailsValidation(string userId, int tagCount, string title, string body)
        {
            // Arrange
            var user = new UserBuilder().BuildUser(new Guid(userId)).Build();
            var tags = new TagBuilder().Build(tagCount);
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();
            var commentable = new Commentable();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
                Question.Create(user, title, body, tags, limits));
        }

        [Theory]
        [InlineData(1)]     // This should be 0, but NBuilder complains about empty lists.
        [InlineData(7)]
        public void Question_CreatingWithWrongNumberOfTags_Fails(int tagCount)
        {
            // Arrange
            var user = new UserBuilder().BuildValidUser().Build();
            var title = "TitleNormal";
            var body = "BodyNormal";
            var tags = Builder<Tag>
                .CreateListOfSize(tagCount)
                .Build()
                .ToList();
            var limits = new LimitsBuilder().Build();
            var voteable = new Voteable();
            var commentable = new Commentable();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
                Question.Create(user, title, body, tags, limits));
        }

        [Fact]
        public void Question_EditingWithValidDataWithinDeadline_Successfully()
        {
            // Arrange - Build Question
            var target = new QuestionBuilder().SetupValidQuestion().Build();

            // Arrange - Edit Data
            string editedTitle = "TitleNormal";
            string editedBody = "BodyNormal";
            int editedTagCount = 5;
            var limits = new LimitsBuilder().Build();
            var editedTags = new TagBuilder().Build(editedTagCount);

            // Act
            target.Edit(target.User, editedTitle, editedBody, editedTags, limits);
            var result = target;

            // Assert
            Assert.NotEqual(default(Guid), result.Id);
            Assert.Equal(editedTitle, result.Title);
            Assert.Equal(editedBody, result.Body);
            Assert.False(result.HasAcceptedAnswer);
            Assert.Empty(result.Answers);
            Assert.Empty(result.Comments);
            Assert.Equal(5, result.QuestionTags.Count());
        }

        [Fact]
        public void Question_EditingOutsideDeadline_Fails()
        {
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
               target.Edit(target.User, editedTitle, editedBody, editedTags, limits));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", 3, "TitleNormal", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "  ", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "  ", "  ")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "123456789", "")]
        [InlineData("00000000-0000-0000-0000-000000000001", 3, "", "123456789")]
        [InlineData("00000000-0000-0000-0000-000000000001", 1, "TitleNormal", "BodyNormal")]
        [InlineData("00000000-0000-0000-0000-000000000001", 6, "TitleNormal", "BodyNormal")]
        public void Question_EditingWithInvalidData_FailsValidation(string userId, int tagCount, string title, string body)
        {
            // Arrange - Build Question
            var user = new UserBuilder().BuildUser(new Guid(userId)).Build();
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var limits = new LimitsBuilder().Build();

            // Arrange - Edit Data
            var editedTags = new TagBuilder().Build(tagCount);

            // Act
            Assert.Throws<BusinessException>(() =>
               target.Edit(user, title, body, editedTags, limits));
        }

        [Fact]
        public void Question_CanBeAnsweredBySameUserOnlyOnce_Sucessfully()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstAnswer = new AnswerBuilder().SetupValidAnswer(target).Build();
            var secondAnswer = new AnswerBuilder().SetupAnotherValidAnswer(target).Build();

            // Act
            target.Answer(firstAnswer);
            target.Answer(secondAnswer);

            // Assert
            Assert.Equal(2, target.Answers.Count());
            Assert.Contains(firstAnswer, target.Answers);
            Assert.Contains(secondAnswer, target.Answers);
        }

        [Fact]
        public void Question_CannotBeAnsweredBySameUserMoreThanOnce_Throws()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstAnswer = new AnswerBuilder().SetupValidAnswer(target, new Guid("00000000-0000-0000-0000-000000000001")).Build();
            var secondAnswer = new AnswerBuilder().SetupValidAnswer(target, new Guid("00000000-0000-0000-0000-000000000001")).Build();
            target.Answer(firstAnswer);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.Answer(secondAnswer));
        }

        [Fact]
        public void Question_OnAnswered_SetHasAcceptedAnswer()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var answer = new AnswerBuilder().SetupValidAnswer(target).Build();
            target.Answer(answer);

            // Act
            target.AcceptAnswer(answer);

            // Assert
            Assert.True(target.HasAcceptedAnswer);
        }

        [Fact]
        public void Question_MoreThanOneAcceptedAnswer_Throws()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstAnswer = new AnswerBuilder().SetupValidAnswer(target).Build();
            var secondAnswer = new AnswerBuilder().SetupAnotherValidAnswer(target).Build();
            target.Answer(firstAnswer);
            target.Answer(secondAnswer);
            target.AcceptAnswer(firstAnswer);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.AcceptAnswer(secondAnswer));
        }

        [Fact]
        public void Question_NonExistingAnswerCannotBeAccepted_Throws()
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstAnswer = new AnswerBuilder().SetupValidAnswer(target).Build();
            var secondAnswer = new AnswerBuilder().SetupAnotherValidAnswer(target).Build();
            target.Answer(firstAnswer);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.AcceptAnswer(secondAnswer));
        }

        [Fact]
        public void Question_UndoAcceptedAnswerWithinDeadline_Successfully()
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var answer = new AnswerBuilder().SetupValidAnswer(target).Build();
            target.Answer(answer);
            target.AcceptAnswer(answer);

            // Act
            target.UndoAcceptAnswer(answer, limits);

            // Assert
            Assert.False(target.HasAcceptedAnswer);
            Assert.False(answer.IsAcceptedAnswer);
            Assert.Null(answer.AcceptedOn);
        }

        [Fact]
        public void Question_UndoAcceptedAnswerOutsideDeadline_Throws()
        {
            // Arrange
            var limits = new LimitsBuilder().Build();
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var answer = new AnswerBuilder().SetupValidAnswer(target).Build();
            target.Answer(answer);
            target.AcceptAnswer(answer);
            // 1 minute past deadline
            answer.SetProperty(
                nameof(answer.AcceptedOn),
                DateTime.UtcNow.AddMinutes(-1 - limits.AcceptAnswerDeadline.Minutes));

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.UndoAcceptAnswer(answer, limits));
        }
    }
}
