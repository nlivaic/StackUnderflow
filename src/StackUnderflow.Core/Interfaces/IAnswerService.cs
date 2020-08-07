using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IAnswerService
    {
        Task AcceptAnswer(AnswerAcceptModel answerModel);
        Task DeleteAnswer(Guid answerUserId, Guid answerId);
        Task EditAnswer(AnswerEditModel answerModel);
        Task PostAnswer(AnswerCreateModel answerModel);
    }
}