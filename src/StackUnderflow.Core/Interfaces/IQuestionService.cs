using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(Guid questionId);
        Task<QuestionGetModel> AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestionAsync(QuestionEditModel questionModel);
        Task DeleteQuestionAsync(Guid questionId, Guid questionUserId);
    }
}
