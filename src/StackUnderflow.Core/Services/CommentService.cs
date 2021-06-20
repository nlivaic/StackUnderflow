using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Common.Caching;

namespace StackUnderflow.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task DeleteRangeAsync(CommentsDeleteModel commentModel)
        {
            IEnumerable<Comment> comments = null;
            if (commentModel.ParentAnswerId.HasValue)
            {
                comments = await
                    _commentRepository.GetCommentsForAnswerAsync(commentModel.ParentAnswerId.Value);
            }
            else if (commentModel.ParentQuestionId.HasValue)
            {
                comments = await
                    _commentRepository.GetCommentsForQuestionAsync<Comment>(commentModel.ParentQuestionId.Value);
            }
            var hasVotes = comments.SelectMany(c => c.Votes).Any();
            if (hasVotes)
            {
                throw new BusinessException($"Cannot delete because associated votes exist on at least one comment.");
            }
            if (comments.Any())
            {
                _commentRepository.Delete(comments);
            }
        }
    }
}
