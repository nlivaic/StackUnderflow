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
        Task<IEnumerable<CommentForQuestionGetModel>> GetCommentsForQuestion(Guid questionId);
        Task<IEnumerable<CommentForAnswerGetModel>> GetCommentsForAnswers(IEnumerable<Guid> answersIds);
        Task<CommentForAnswerGetModel> GetCommentForAnswer(Guid questionId, Guid answerId, Guid commentId);
        Task<CommentForQuestionGetModel> GetCommentModel(Guid questionId, Guid commentId);
        Task<Comment> GetCommentWithUser(Guid commentId);
        Task<Comment> GetCommentWithAnswer(Guid commentId);
    }
}
