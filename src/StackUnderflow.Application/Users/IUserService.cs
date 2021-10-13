using StackUnderflow.Application.Users.Models;
using System;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.Users
{
    public interface IUserService
    {
        Task<UserGetModel> GetUserAsync(Guid userId);
        Task<UserGetModel> CreateUserAsync(UserCreateModel user);
        Task<bool> IsModeratorAsync(Guid? userId);
    }
}
