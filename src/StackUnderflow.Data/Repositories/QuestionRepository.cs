using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;
using StackUnderflow.Data.QueryableExtensions;
using StackUnderflow.WorkerServices.PointServices.Sorting.Models;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Application.Questions.Models;

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

        public async Task<QuestionGetModel> GetQuestionWithUserAndTagsAsync(Guid id, Guid? currentUserId)
        {
            var q = await _context
                .Questions
                .Where(q => q.Id == id)
                .Include(q => q.User)
                .ThenInclude(u => u.Roles)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.Votes.Where(v => v.UserId == currentUserId))
                .AsNoTracking()
                // Below line commented out because it was messing up
                // votes - all votes would be included no matter
                // the user id value (or lack of).
                //.ProjectTo<QuestionGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            return _mapper.Map<QuestionGetModel>(q);
        }

        public async Task<PagedList<QuestionSummaryGetModel>> GetQuestionSummariesAsync(QuestionQueryParameters questionQueryParameters)
        {
            var query = _context
                .Questions as IQueryable<Question>;
            if (questionQueryParameters.Tags.Any())
            {
                query = query.Where(q => q.QuestionTags.Any(qt => questionQueryParameters.Tags.Contains(qt.TagId)));
            }
            if (questionQueryParameters.Users.Any())
            {
                query = query.Where(q => questionQueryParameters.Users.Any(a => a == q.UserId));
            }
            if (!string.IsNullOrWhiteSpace(questionQueryParameters.SearchQuery))
            {
                var searchQueryLowercase = questionQueryParameters.SearchQuery.ToLower();
                query = query.Where(q =>
                    q.Title.ToLower().Contains(searchQueryLowercase) ||
                    q.Body.ToLower().Contains(searchQueryLowercase));
            }
            return await query
                // .OrderBy(q => q.Id)           // @nl: ordering on Guid. Think this through!
                .Include(q => q.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .ApplySorting(questionQueryParameters.SortBy)
                .AsNoTracking()
                .ApplyPagingAsync<Question, QuestionSummaryGetModel>(_mapper, questionQueryParameters.PageNumber, questionQueryParameters.PageSize);
        }

        public async Task<Question> GetQuestionWithAnswersAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync();


        public async Task<Question> GetQuestionWithAnswerAsync(Guid questionId, Guid answerId) =>
            await _context
                .Questions
                .Include(q => q.Answers.Where(a => a.Id == answerId))
                .Where(q => q.Id == questionId)
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
                .Include(q => q.Comments)
                .Where(q => q.Id == questionId)
                .SingleOrDefaultAsync();

        public async Task<Question> GetQuestionWithTagsAsync(Guid questionId) =>
            await _context
                .Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .SingleOrDefaultAsync();
    }
}
