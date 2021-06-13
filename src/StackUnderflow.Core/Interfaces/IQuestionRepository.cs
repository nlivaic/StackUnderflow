using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Questions;
using StackUnderflow.Application.Services.Sorting;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(QuestionFindModel questionFindModel);
        Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummariesAsync(QuestionQueryParameters questionQueryParameters);
        Task<Question> GetQuestionWithAnswersAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithTagsAsync(Guid questionId);
    }
}
