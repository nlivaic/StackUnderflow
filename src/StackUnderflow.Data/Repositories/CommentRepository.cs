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

        public async Task<IEnumerable<CommentGetModel>> GetCommentsForQuestion(Guid questionId) =>
            await _context
                .Comments
                .Where(c => c.ParentQuestionId == questionId)
                .ProjectTo<CommentGetModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
