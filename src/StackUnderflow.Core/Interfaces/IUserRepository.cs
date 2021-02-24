using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<T> GetUser<T>(Guid userId);
        Task<bool> IsModeratorAsync(Guid userId);
    }
}
