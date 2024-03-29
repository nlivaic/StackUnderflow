using System;
using System.Threading.Tasks;
using AutoMapper;
using StackUnderflow.Application.Users.Models;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.WorkerServices.Users;

namespace StackUnderflow.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILimits _limits;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILimits limits)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _limits = limits;
        }

        public async Task<UserGetModel> GetUserAsync(Guid userId) =>
            await _userRepository.GetUser<UserGetModel>(userId);

        public async Task<UserGetModel> CreateUserAsync(UserCreateModel model)
        {
            var user = User.Create(_limits, model.Id, model.Username, model.Email);
            await _userRepository.AddAsync(user);
            var userGetModel = _mapper.Map<UserGetModel>(user);
            return userGetModel;
        }

        public async Task<bool> IsModeratorAsync(Guid? userId) =>
            userId != null &&
            userId != default(Guid) &&
            await _userRepository.IsModeratorAsync(userId.Value);
    }
}
