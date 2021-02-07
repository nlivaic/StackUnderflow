using System;
using System.Threading.Tasks;
using AutoMapper;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILimits _limits;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork uow,
            IMapper mapper,
            ILimits limits)
        {
            _userRepository = userRepository;
            _uow = uow;
            _mapper = mapper;
            _limits = limits;
        }

        public async Task<UserGetModel> GetUserAsync(Guid userId) =>
            await _userRepository.GetUser(userId);

        public async Task<UserGetModel> CreateUserAsync(UserCreateModel model)
        {
            var user = User.Create(_limits, model.Id, model.Username, model.Email);
            await _userRepository.AddAsync(user);
            await _uow.SaveAsync();
            var userGetModel = _mapper.Map<UserGetModel>(user);
            return userGetModel;
        }
    }
}
