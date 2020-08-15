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
        Task<IEnumerable<CommentForQuestionGetModel>> GetCommentsForQuestionAsync(Guid questionId);
        Task<IEnumerable<CommentForAnswerGetModel>> GetCommentsForAnswersAsync(IEnumerable<Guid> answersIds);
        Task<CommentForAnswerGetModel> GetCommentForAnswerAsync(Guid answerId, Guid commentId);
        Task<CommentForQuestionGetModel> GetCommentModelAsync(Guid questionId, Guid commentId);
        Task<Comment> GetCommentWithUserAsync(Guid commentId);
        Task<Comment> GetCommentWithAnswerAsync(Guid commentId);
    }
}
