using System.Collections.Generic;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentable
    {
        IEnumerable<Comment> Comments { get; }

        void Comment(Comment comment);
    }
}