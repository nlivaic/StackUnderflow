using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentService
    {
        Task<CommentGetModel> CommentOnQuestionAsync(CommentOnQuestionCreateModel commentModel);
        Task EditAsync(CommentEditModel commentModel);
    }
}
