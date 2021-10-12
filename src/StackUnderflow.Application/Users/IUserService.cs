using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.WorkerServices.Users
{
    public interface IUserService
    {
        Task<UserGetModel> GetUserAsync(Guid userId);
        Task<UserGetModel> CreateUserAsync(UserCreateModel user);
        Task<bool> IsModeratorAsync(Guid? userId);
    }
}
