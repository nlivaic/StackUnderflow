using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Collections;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.QueryParameters;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<Answer> GetAnswerWithCommentsAsync(Guid questionId, Guid answerId);
        Task<CommentForAnswerGetModel> GetCommentModel(Guid questionId, Guid answerId, Guid commentId);
    }
}
