using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Core.Entities;
using StackUnderflow.Common.Extensions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly IMapper _mapper;

        public CommentRepository(StackUnderflowDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetCommentsForQuestionAsync<T>(Guid questionId) =>
            await _context
                .Comments
                .Where(c => c.ParentQuestionId == questionId)
                .Projector<T>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<CommentForAnswerGetModel> GetCommentForAnswerAsync(Guid answerId, Guid commentId) =>
            await _context
                .Comments
                .Include(c => c.ParentAnswer)
                .ThenInclude(a => a.Question)
                .Where(c => c.ParentAnswerId == answerId && c.Id == commentId)
                .ProjectTo<CommentForAnswerGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Comment>> GetCommentsForAnswerAsync(Guid answerId) =>
            await
                CommentsForAnswerQuery(c =>
                    c.ParentAnswerId.HasValue && c.ParentAnswerId.Value == answerId)
                .ToListAsync();

        public async Task<IEnumerable<CommentForAnswerGetModel>> GetCommentsForAnswersAsync(IEnumerable<Guid> answerIds) =>
            await
                CommentsForAnswerQuery(c =>
                    c.ParentAnswerId.HasValue && answerIds.Contains(c.ParentAnswerId.Value))
                .ProjectTo<CommentForAnswerGetModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

        private IQueryable<Comment> CommentsForAnswerQuery(Expression<Func<Comment, bool>> predicate) =>
            _context
                .Comments
                .Where(predicate)
                .OrderBy(c => c.ParentAnswerId)
                .ThenBy(c => c.OrderNumber);

        public async Task<CommentForQuestionGetModel> GetCommentModelAsync(Guid questionId, Guid commentId) =>
            await _context
                .Comments
                .Where(c => c.ParentQuestionId == questionId && c.Id == commentId)
                .ProjectTo<CommentForQuestionGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<Comment> GetCommentWithUserAsync(Guid commentId) =>
            await _context
                .Comments
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == commentId);

        public async Task<Comment> GetCommentWithAnswerAsync(Guid commentId) =>
            await _context
                .Comments
                .Include(c => c.ParentAnswer)
                .Include(c => c.User)
                .ThenInclude(u => u.Roles)
                .SingleOrDefaultAsync(c => c.Id == commentId);

        public async Task<Comment> GetCommentWithQuestionAsync(Guid commentId) =>
            await _context
                .Comments
                .Include(c => c.ParentQuestion)
                .Include(c => c.User)
                .ThenInclude(u => u.Roles)
                .SingleOrDefaultAsync(c => c.Id == commentId);
    }
}
