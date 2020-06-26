using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> GetQuestionWithUserAndAllCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithCommentsAsync(Guid questionId);
    }
}