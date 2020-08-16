using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IAnswerService
    {
        Task<AnswerGetModel> AcceptAnswerAsync(AnswerAcceptModel answerModel);
        Task DeleteAnswerAsync(Guid answerUserId, Guid questionId, Guid answerId);
        Task EditAnswerAsync(AnswerEditModel answerModel);
        Task<AnswerGetModel> PostAnswerAsync(AnswerCreateModel answerModel);
    }
}