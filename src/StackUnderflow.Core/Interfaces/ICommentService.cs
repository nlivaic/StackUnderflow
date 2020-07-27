using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentService
    {
        Task<CommentForQuestionGetModel> CommentOnQuestionAsync(CommentOnQuestionCreateModel commentModel);
        Task<CommentForAnswerGetModel> CommentOnAnswerAsync(CommentOnAnswerCreateModel commentModel);
        Task EditAsync(CommentEditModel commentModel);
        Task DeleteAsync(CommentDeleteModel commentModel);
    }
}
