using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Data.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(StackUnderflowDbContext context)
            : base(context)
        { }

        public async Task<Question> GetQuestionWithUserAndAllCommentsAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.User)
                .Include(q => q.Comments)
                .ThenInclude(c => c.User)
                .SingleOrDefaultAsync();

        public async Task<Question> GetQuestionWithAnswersAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync();

        public async Task<Question> GetQuestionWithAnswersAndCommentsAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Answers)
                .Include(q => q.Comments)
                .SingleOrDefaultAsync();
        public async Task<Question> GetQuestionWithCommentsAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Comments)
                .SingleOrDefaultAsync();
    }
}
