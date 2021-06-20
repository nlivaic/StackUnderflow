using System.Threading.Tasks;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface ICommentService
    {
        Task DeleteRangeAsync(CommentsDeleteModel commentModel);
    }
}
