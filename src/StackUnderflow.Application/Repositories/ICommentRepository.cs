using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackUnderflow.Application.Comments.Models;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<T>> GetCommentsForQuestionAsync<T>(Guid questionId);
        Task<IEnumerable<Comment>> GetCommentsForAnswerAsync(Guid answerId);
        Task<IEnumerable<CommentForAnswerGetModel>> GetCommentsForAnswersAsync(IEnumerable<Guid> answersIds);
        Task<CommentForAnswerGetModel> GetCommentForAnswerAsync(Guid answerId, Guid commentId);
        Task<CommentForQuestionGetModel> GetCommentModelAsync(Guid questionId, Guid commentId);
        Task<Comment> GetCommentWithUserAsync(Guid commentId);
        Task<Comment> GetCommentWithAnswerAsync(Guid commentId);
        Task<Comment> GetCommentWithQuestionAsync(Guid commentId);
    }
}
