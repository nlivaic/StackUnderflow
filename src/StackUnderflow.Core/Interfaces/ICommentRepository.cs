using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<CommentGetModel>> GetCommentsForQuestion(Guid questionId);
        Task<CommentGetModel> GetCommentModel(Guid questionId, Guid commentId);
        Task<Comment> GetCommentWithUser(Guid commentId);
    }
}
