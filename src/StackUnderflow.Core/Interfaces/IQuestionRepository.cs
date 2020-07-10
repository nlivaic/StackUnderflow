using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.QueryParameters;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(Guid questionId);
        Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummaries(QuestionQueryParameters questionQueryParameters);
        Task<Question> GetQuestionWithAnswersAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithCommentsAsync(Guid questionId);
    }
}
