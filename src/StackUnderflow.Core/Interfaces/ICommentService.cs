using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentService
    {
        Task CommentOnQuestion(CommentCreateModel commentModel);
        Task Edit(CommentEditModel commentModel);
    }
}