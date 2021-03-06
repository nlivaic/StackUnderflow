using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Data.Repositories
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(StackUnderflowDbContext context)
            : base(context)
        { }

        public async Task<Vote> GetVoteAsync(Guid voteUserId, Guid voteId) =>
            await _context
                .Votes
                .SingleOrDefaultAsync(v => v.UserId == voteUserId && v.Id == voteId);

        public async Task<Vote> GetVoteAsync(Guid voteUserId, Guid? questionId, Guid? answerId, Guid? commentId)
        {
            IQueryable<Vote> query = _context
                .Votes;
            if (questionId.HasValue)
            {
                query = query.Include(v => v.Question);
            }
            else if (answerId.HasValue)
            {
                query = query.Include(v => v.Answer);
            }
            else if (commentId.HasValue)
            {
                query = query.Include(v => v.Comment);
            }
            else
            {
                throw new BusinessException("At least one linked identifier must be provided when querying for votes.");
            }
            return await query
                .Where(v => v.QuestionId == questionId && v.UserId == voteUserId)
                .SingleOrDefaultAsync();
        }
    }
}