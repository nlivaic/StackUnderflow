using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionSummaryGetModel>> GetQuestionSummaries();
        Task<QuestionGetModel> GetQuestionWithUserAndAllCommentsAsync(Guid questionId);
        Task AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestionAsync(QuestionEditModel questionModel);
        Task DeleteQuestionAsync(Guid questionUserId, Guid questionId);
    }
}