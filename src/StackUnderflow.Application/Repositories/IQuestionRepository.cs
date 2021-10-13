using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Common.Paging;
using StackUnderflow.Application.Questions.Models;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(Guid id, Guid? currentUserId);
        Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummariesAsync(QuestionQueryParameters questionQueryParameters);
        Task<Question> GetQuestionWithAnswersAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithTagsAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswerAsync(Guid questionId, Guid answerId);
    }
}
