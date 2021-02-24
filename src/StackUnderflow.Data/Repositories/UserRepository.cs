using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Extensions;
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

        public async Task<T> GetUser<T>(Guid userId) =>
            await _context
                .Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .Projector<T>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<bool> IsModeratorAsync(Guid userId) =>
            await _context
                .Users
                .AnyAsync(u => u.Id == userId && u.Roles.Any(ur => ur.Role == Role.Moderator));
    }
}