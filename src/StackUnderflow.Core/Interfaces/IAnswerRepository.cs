using System;
using System.Threading.Tasks;
using StackUnderflow.Application.Services.Sorting.Models;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<AnswerGetModel> GetAnswerWithUserAsync(Guid questionId, Guid answerId);
        Task<PagedList<AnswerGetModel>> GetAnswersWithUserAsync(Guid questionId, AnswerQueryParameters queryParameters);
        Task<Answer> GetAnswerWithCommentsAsync(Guid questionId, Guid answerId);
        Task<CommentForAnswerGetModel> GetCommentModelAsync(Guid questionId, Guid answerId, Guid commentId);
        Task<Answer> GetAnswerWithCommentsAndVotesAsync(Guid questionId, Guid answerId);
    }
}
