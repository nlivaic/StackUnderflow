using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IAnswerService
    {
        Task DeleteAnswerAsync(Answer answer, int votesSum);
    }
}