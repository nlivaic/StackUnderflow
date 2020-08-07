using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Core.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<AnswerGetModel> GetAnswerWithUserAsync(Guid questionId, Guid answerId);
        Task<PagedList<AnswerGetModel>> GetAnswersWithUserAsync(Guid questionId, AnswerQueryParameters queryParameters);
        Task<Answer> GetAnswerWithCommentsAsync(Guid questionId, Guid answerId);
        Task<CommentForAnswerGetModel> GetCommentModel(Guid questionId, Guid answerId, Guid commentId);
    }
}
