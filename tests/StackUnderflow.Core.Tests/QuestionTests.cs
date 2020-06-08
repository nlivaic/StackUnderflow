using System;
using System.Linq;
using FizzWare.NBuilder;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
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
            Guid ownerId = new Guid("00000000-0000-0000-0000-000000000001");
            int tagCount = 3;
            string title = "TitleNormal";
            string body = "BodyNormal";
            var tags = new TagBuilder().Build(tagCount);
            var limits = new LimitsBuilder().Build();

            // Act
            var result = Question.Create(ownerId, title, body, tags, limits);

            // Assert
            Assert.NotEqual(default(Guid), result.Id);
            Assert.Equal(ownerId, result.OwnerId);
            Assert.Equal(title, result.Title);
            Assert.Equal(body, result.Body);
            Assert.False(result.HasAcceptedAnswer);
            Assert.True(DateTime.UtcNow - result.CreatedOn < TimeSpan.FromSeconds(1));
            Assert.Empty(result.Answers);
            Assert.Empty(result.Comments);
            Assert.Equal(3, result.Tags.Count());
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
        public void Question_CreatingWithInvalidData_FailsValidation(string ownerId, int tagCount, string title, string body)
        {
            // Arrange
            var tags = new TagBuilder().Build(tagCount);
            var limits = new LimitsBuilder().Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
                Question.Create(new Guid(ownerId), title, body, tags, limits));
        }

        [Theory]
        [InlineData(1)]     // This should be 0, but NBuilder complains about empty lists.
        [InlineData(7)]
        public void Question_CreatingWithWrongNumberOfTags_Fails(int tagCount)
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var title = "TitleNormal";
            var body = "BodyNormal";
            var tags = Builder<Tag>
                .CreateListOfSize(tagCount)
                .Build()
                .ToList();
            var limits = new LimitsBuilder().Build();

            // Act, Assert
            Assert.Throws<BusinessException>(() =>
                Question.Create(ownerId, title, body, tags, limits));
        }

        [Fact]
        public void Question_CanBeEditedWithinDeadline_Successfully()
        {
            // Arrange - Build Question
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var ownerId = target.OwnerId;

            // Arrange - Edit Data
            string editedTitle = "TitleNormal";
            string editedBody = "BodyNormal";
            int editedTagCount = 5;
            var limits = new LimitsBuilder().Build();
            var editedTags = new TagBuilder().Build(editedTagCount);

            // Act
            target.Edit(target.OwnerId, editedTitle, editedBody, editedTags, limits);
            var result = target;

            // Assert
            Assert.NotEqual(default(Guid), result.Id);
            Assert.Equal(ownerId, result.OwnerId);
            Assert.Equal(editedTitle, result.Title);
            Assert.Equal(editedBody, result.Body);
            Assert.False(result.HasAcceptedAnswer);
            Assert.Empty(result.Answers);
            Assert.Empty(result.Comments);
            Assert.Equal(5, result.Tags.Count());
        }

        [Fact]
        public void Question_EditedOutsideDeadline_Fails()
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
               target.Edit(target.OwnerId, editedTitle, editedBody, editedTags, limits));
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
        public void Question_EditedWithInvalidData_FailsValidation(string ownerId, int tagCount, string title, string body)
        {
            // Arrange - Build Question
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var limits = new LimitsBuilder().Build();

            // Arrange - Edit Data
            var editedTags = new TagBuilder().Build(tagCount);

            // Act
            Assert.Throws<BusinessException>(() =>
               target.Edit(new Guid(ownerId), title, body, editedTags, limits));
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
            var firstAnswer = new AnswerBuilder().SetupValidAnswer(target).Build();
            var secondAnswer = new AnswerBuilder().SetupValidAnswer(target).Build();
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
            target.AcceptAnswer();

            // Assert
            Assert.True(target.HasAcceptedAnswer);
        }

        [Fact]
        public void Question_CommentsAreAddedInIncreasingOrder_Successfully()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstComment = new CommentBuilder().SetupValidComment(1).Build();
            var secondComment = new CommentBuilder().SetupValidComment(2).Build();
            var thirdComment = new CommentBuilder().SetupValidComment(7).Build();  // Not back-to-back.

            // Act
            target.Comment(firstComment);
            target.Comment(secondComment);
            target.Comment(thirdComment);

            // Assert
            Assert.Equal(3, target.Comments.Count());
            Assert.Contains(firstComment, target.Comments);
            Assert.Equal(1, firstComment.OrderNumber);
            Assert.Contains(secondComment, target.Comments);
            Assert.Equal(2, secondComment.OrderNumber);
            Assert.Contains(thirdComment, target.Comments);
            Assert.Equal(7, thirdComment.OrderNumber);
        }

        [Fact]
        public void Question_CommentOutOfOrder_Throws()
        {
            // Arrange
            var target = new QuestionBuilder().SetupValidQuestion().Build();
            var firstComment = new CommentBuilder().SetupValidComment(1).Build();
            var secondComment = new CommentBuilder().SetupValidComment(2).Build();
            var targetComment = new CommentBuilder().SetupValidComment(2).Build();  // Repeat the order number.
            target.Comment(firstComment);
            target.Comment(secondComment);

            // Act, Assert
            Assert.Throws<BusinessException>(() => target.Comment(targetComment));
        }
    }
}