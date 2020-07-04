using System;
using System.Collections.Generic;
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
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly IMapper _mapper;

        public QuestionRepository(StackUnderflowDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<QuestionGetModel> GetQuestionWithUserAndAllCommentsAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.User)
                .Include(q => q.Comments)
                .ThenInclude(c => c.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .ProjectTo<QuestionGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<QuestionSummaryGetModel>> GetQuestionSummaries() =>
            await _context
                .Questions
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .ProjectTo<QuestionSummaryGetModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

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
