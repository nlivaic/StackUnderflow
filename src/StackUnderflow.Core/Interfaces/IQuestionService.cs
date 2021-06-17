using System.Threading.Tasks;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Core.Interfaces
{
    public interface IQuestionService
    {
        Task DeleteQuestionAsync(Question question, int votesSum);
    }
}
