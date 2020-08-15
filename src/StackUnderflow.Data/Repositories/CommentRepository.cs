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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly IMapper _mapper;

        public CommentRepository(StackUnderflowDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentForQuestionGetModel>> GetCommentsForQuestionAsync(Guid questionId) =>
            await _context
                .Comments
                .Where(c => c.ParentQuestionId == questionId)
                .ProjectTo<CommentForQuestionGetModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<CommentForAnswerGetModel> GetCommentForAnswerAsync(Guid answerId, Guid commentId) =>
            await _context
                .Comments
                .Include(c => c.ParentAnswer)
                .ThenInclude(a => a.Question)
                .Where(c => c.ParentAnswerId == answerId && c.Id == commentId)
                .ProjectTo<CommentForAnswerGetModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<CommentForAnswerGetModel>> GetCommentsForAnswersAsync(IEnumerable<Guid> answerIds) =>
            await _context
                .Comments
                .Where(c => c.ParentAnswerId.HasValue && answerIds.Contains(c.ParentAnswerId.Value))
                .OrderBy(c => c.ParentAnswerId)
                .ThenBy(c => c.OrderNumber)
                .ProjectTo<CommentForAnswerGetModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

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
                .SingleOrDefaultAsync(c => c.Id == commentId);



    }
}
