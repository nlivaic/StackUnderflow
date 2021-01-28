using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserGetModel> GetUser(Guid userId);
    }
}
