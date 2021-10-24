using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using StackUnderflow.Common.Extensions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

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
            await Context
                .Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .Projector<T>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<bool> IsModeratorAsync(Guid userId) =>
            await Context
                .Users
                .AnyAsync(u => u.Id == userId && u.Roles.Any(ur => ur.Role == Role.Moderator));

        public async Task CalculatePointsAsync(Guid userId, int pointAmount)
        {
            var userIdParam = new NpgsqlParameter("user_id", NpgsqlDbType.Uuid) { Value = userId };
            var pointAmountParam = new NpgsqlParameter("points_amount", NpgsqlDbType.Integer) { Value = pointAmount };

            await Context.Database.ExecuteSqlRawAsync(
                @"UPDATE ""Users""" +
                @"   SET ""Points"" = (SELECT ""Points"" FROM ""Users"" WHERE ""Id"" = @user_id) + @points_amount" +
                @" WHERE ""Id"" = @user_id",
                userIdParam,
                pointAmountParam);
        }
    }
}