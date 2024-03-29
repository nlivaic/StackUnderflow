using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Application.Comments.Models;
using StackUnderflow.Application.Sorting.Models;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Data.QueryableExtensions;

namespace StackUnderflow.Data.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly IMapper _mapper;

        public AnswerRepository(StackUnderflowDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<AnswerGetModel> GetAnswerWithUserAsync(Guid questionId, Guid answerId) =>
            await Context
                .Answers
                .Include(a => a.User)
                .Where(a => a.QuestionId == questionId && a.Id == answerId)
                .ProjectTo<AnswerGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<PagedList<AnswerGetModel>> GetAnswersWithUserAsync(Guid questionId, AnswerQueryParameters queryParameters)
        {
            return await Context
                .Answers
                .Include(a => a.User)
                .Where(a => a.QuestionId == questionId)
                .ApplySorting(queryParameters.SortBy)
                .AsNoTracking()
                .ApplyPagingAsync<Answer, AnswerGetModel>(_mapper, queryParameters.PageNumber, queryParameters.PageSize);
        }

        public async Task<CommentForAnswerGetModel> GetCommentModelAsync(Guid questionId, Guid answerId, Guid commentId) =>
            await Context
                .Comments
                .Where(c => c.ParentQuestionId == questionId && c.ParentAnswerId == answerId && c.Id == commentId)
                .ProjectTo<CommentForAnswerGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<Answer> GetAnswerWithCommentsAsync(Guid questionId, Guid answerId) =>
            await Context
                .Answers
                .Include(q => q.Comments)
                .Where(a => a.Id == answerId && a.QuestionId == questionId)
                .SingleOrDefaultAsync();

        public async Task<Answer> GetAnswerWithCommentsAndVotesAsync(Guid questionId, Guid answerId) =>
            await Context
                .Answers
                .Include(a => a.Votes.Take(1))
                .Include(q => q.Comments)
                .ThenInclude(c => c.Votes.Take(1))
                .Where(a => a.Id == answerId && a.QuestionId == questionId)
                .SingleOrDefaultAsync();
    }
}
