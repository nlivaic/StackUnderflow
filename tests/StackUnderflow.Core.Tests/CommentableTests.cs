using System;
using System.Linq;
using FizzWare.NBuilder;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Tests.Builders;
using Xunit;

namespace StackUnderflow.Core.Tests
{
    public class CommentableTests
    {
        [Fact]
        public void Commentable_CommentsAreAddedInIncreasingOrder_Successfully()
        {
            // Arrange
            var target = new Commentable();
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
            var target = new Commentable();
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