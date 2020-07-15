using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Core.QueryParameters;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummaries(QuestionQueryParameters questionQueryParameters);
        Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(Guid questionId);
        Task<QuestionGetModel> AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestionAsync(QuestionEditModel questionModel);
        Task DeleteQuestionAsync(Guid questionUserId, Guid questionId);
    }
}