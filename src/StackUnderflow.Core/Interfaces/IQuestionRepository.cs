using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> GetQuestionWithAnswersAsync(Guid questionId);
        Task<QuestionModel> GetQuestionWithAnswersAndAllCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithCommentsAsync(Guid questionId);
    }
}