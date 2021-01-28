using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IMapper _mapper;

        public UserRepository(
            StackUnderflowDbContext context,
            IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<UserGetModel> GetUser(Guid userId) =>
            await _context
                .Users
                .ProjectTo<UserGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(u => u.Id == userId);
    }
}