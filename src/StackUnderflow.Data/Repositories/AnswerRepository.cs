using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Collections;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.QueryParameters;
using StackUnderflow.Data.QueryableExtensions;
using System.Linq.Dynamic.Core;

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
            await _context
                .Answers
                .Include(a => a.User)
                .Where(a => a.QuestionId == questionId && a.Id == answerId)
                .ProjectTo<AnswerGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<PagedList<AnswerGetModel>> GetAnswersWithUserAsync(Guid questionId, AnswerQueryParameters queryParameters)
        {
            return await _context
                .Answers
                .Include(a => a.User)
                .Where(a => a.QuestionId == questionId)
                .ApplySorting(queryParameters.SortBy)
                .AsNoTracking()
                .ApplyPagingAsync<Answer, AnswerGetModel>(_mapper, queryParameters.PageNumber, queryParameters.PageSize);
        }

        public async Task<CommentForAnswerGetModel> GetCommentModelAsync(Guid questionId, Guid answerId, Guid commentId) =>
            await _context
                .Comments
                .Where(c => c.ParentQuestionId == questionId && c.ParentAnswerId == answerId && c.Id == commentId)
                .ProjectTo<CommentForAnswerGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<Answer> GetAnswerWithCommentsAsync(Guid questionId, Guid answerId) =>
            await _context
                .Answers
                .Include(q => q.Comments)
                .Where(a => a.Id == answerId && a.QuestionId == questionId)
                .SingleOrDefaultAsync();
    }
}
