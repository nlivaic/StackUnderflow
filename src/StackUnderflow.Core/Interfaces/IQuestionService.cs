using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    interface IQuestionService
    {
        Task<QuestionModel> GetQuestion(Guid questionId);
        Task AskQuestionAsync(QuestionCreateModel questionModel);
        Task EditQuestion(QuestionEditModel questionModel);
        Task DeleteQuestion(Guid questionOwnerId, Guid questionId);
    }
}