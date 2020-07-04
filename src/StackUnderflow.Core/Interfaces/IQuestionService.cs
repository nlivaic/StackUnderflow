using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionWithUserAndAllCommentsModel> GetQuestionWithUserAndAllCommentsAsync(Guid questionId);
        Task AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestionAsync(QuestionEditModel questionModel);
        Task DeleteQuestionAsync(Guid questionUserId, Guid questionId);
    }
}