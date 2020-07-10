using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Collections;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.QueryParameters;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Data.QueryableExtensions;

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

        public async Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .AsNoTracking()
                .ProjectTo<QuestionGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummaries(QuestionQueryParameters questionQueryParameters)
        {
            var query = _context
                .Questions as IQueryable<Question>;
            if (questionQueryParameters.Tags.Any())
            {
                query = query.Where(q => q.QuestionTags.Any(qt => questionQueryParameters.Tags.Contains(qt.TagId)));
            }
            if (questionQueryParameters.Authors.Any())
            {
                query = query.Where(q => questionQueryParameters.Authors.Any(a => a == q.UserId));
            }
            if (!string.IsNullOrWhiteSpace(questionQueryParameters.SearchQuery))
            {
                var searchQueryLowercase = questionQueryParameters.SearchQuery.ToLower();
                query = query.Where(q =>
                    q.Title.ToLower().Contains(searchQueryLowercase) ||
                    q.Body.ToLower().Contains(searchQueryLowercase));
            }
            query = query
                .OrderBy(q => q.Id)           // @nl: ordering on Guid. Think this through!
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .AsNoTracking();
            return await query
                .ApplyPagingAsync<Question, QuestionSummaryGetModel>(_mapper, questionQueryParameters.PageNumber, questionQueryParameters.PageSize);
        }

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
