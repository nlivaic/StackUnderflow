using System;
using System.Threading.Tasks;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<UserGetModel> GetUserAsync(Guid userId) =>
            await _userRepository.GetUser(userId);
    }
}
