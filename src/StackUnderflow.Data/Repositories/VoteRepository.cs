using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Data.Repositories
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(StackUnderflowDbContext context)
            : base(context)
        {
        }

        public async Task<Vote> GetVoteAsync(Guid voteUserId, Guid voteId) =>
            await Context
                .Votes
                .SingleOrDefaultAsync(v => v.UserId == voteUserId && v.Id == voteId);

        public async Task<Vote> GetVoteWithTargetAsync(Guid voteUserId, Guid voteId) =>
            await Context
                .Votes
                .Include(v => v.Question)
                .Include(v => v.Answer)
                .Include(v => v.Comment)
                .SingleOrDefaultAsync(v => v.UserId == voteUserId && v.Id == voteId);

        public async Task<int> GetVotesSumAsync(Guid targetId)
        {
            return await Context
                .Votes
                .Where(v => (v.QuestionId == null || v.QuestionId == targetId)
                    && (v.AnswerId == null || v.AnswerId == targetId)
                    && (v.CommentId == null || v.CommentId == targetId))
                .GroupBy(v => v.VoteType)
                .Select(g => g.Key == VoteTypeEnum.Upvote ? g.Count() : -1 * g.Count())
                .SumAsync();
        }
    }
}
