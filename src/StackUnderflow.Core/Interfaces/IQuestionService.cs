using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Questions;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(QuestionFindModel questionFindModel);
        Task<QuestionGetModel> AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestionAsync(QuestionEditModel questionModel);
        Task DeleteQuestionAsync(Guid questionId, Guid questionUserId);
    }
}
