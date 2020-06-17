using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Core.Entities
{
    public class Commentable : ICommentable
    {
        public IEnumerable<Comment> Comments => _comments;

        private List<Comment> _comments = new List<Comment>();

        public void Comment(Comment comment)
        {
            var lastOrderNumber = Comments
                .Select(c => c.OrderNumber)
                .OrderByDescending(c => c)
                .FirstOrDefault();
            if (lastOrderNumber >= comment.OrderNumber)
            {
                throw new BusinessException("Comment must be added as last in order.");
            }
            _comments.Add(comment);
        }
    }
}