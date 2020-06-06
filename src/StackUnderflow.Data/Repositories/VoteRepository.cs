using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

        public async Task<Vote> GetVote(Guid voteOwnerId, Guid? questionId, Guid? answerId, Guid? commentId)
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
                throw new ArgumentException("At least one linked identifier must be provided when querying for votes.");
            }
            return await query
                .Where(v => v.QuestionId == questionId && v.OwnerId == voteOwnerId)
                .SingleOrDefaultAsync();
        }
    }
}