using System;
using System.Threading.Tasks;
using StackUnderflow.WorkerServices.PointServices.Sorting.Models;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Entities;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Application.Comments.Models;

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
