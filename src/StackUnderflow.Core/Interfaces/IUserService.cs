using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserGetModel> GetUserAsync(Guid userId);
        Task<UserGetModel> CreateUserAsync(UserCreateModel user);
        Task<bool> IsModeratorAsync(Guid userId);
    }
}
