using System;
using System.Threading.Tasks;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<UserGetModel> GetUser(Guid userId);
    }
}
