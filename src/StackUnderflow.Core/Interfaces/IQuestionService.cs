using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Core.Foo;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummaries(QuestionResourceParameters questionResourceParameters);
        Task<QuestionGetModel> GetQuestionWithUserAndAllCommentsAsync(Guid questionId);
        Task AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestionAsync(QuestionEditModel questionModel);
        Task DeleteQuestionAsync(Guid questionUserId, Guid questionId);
    }
}