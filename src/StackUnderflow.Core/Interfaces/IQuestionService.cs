using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    interface IQuestionService
    {
        Task AskQuestionAsync(Guid ownerId, string title, string body, IEnumerable<Guid> tagIds);
        Task EditQuestion(QuestionEditModel questionModel);
    }
}