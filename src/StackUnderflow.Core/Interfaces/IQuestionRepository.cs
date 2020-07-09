using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Foo;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<QuestionGetModel> GetQuestionWithUserAndAllCommentsAsync(Guid questionId);
        Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummaries(QuestionResourceParameters questionResourceParameters);
        Task<Question> GetQuestionWithAnswersAsync(Guid questionId);
        Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId);
        Task<Question> GetQuestionWithCommentsAsync(Guid questionId);
    }
}
