﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Application.Comments.Commands
{
    public class DeleteCommentOnAnswerCommand : IRequest<Unit>
    {
        public Guid ParentQuestionId { get; set; }
        public Guid ParentAnswerId { get; set; }
        public Guid CommentId { get; set; }
        public Guid CurrentUserId { get; set; }

        private class DeleteCommentOnAnswerCommandHandler : IRequestHandler<DeleteCommentOnAnswerCommand, Unit>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IVoteRepository _voteRepository;

            public DeleteCommentOnAnswerCommandHandler(
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                IVoteRepository voteRepository)
            {
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _voteRepository = voteRepository;
            }

            public async Task<Unit> Handle(DeleteCommentOnAnswerCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentWithAnswerAsync(request.CommentId);
                if (comment == null
                    || comment.ParentAnswerId != request.ParentAnswerId
                    || comment.ParentAnswer.QuestionId != request.ParentQuestionId)
                {
                    throw new EntityNotFoundException(nameof(Comment), request.CommentId);
                }
                var user = await _userRepository.GetUser<User>(request.CurrentUserId);
                var votesSum = await _voteRepository.CountAsync(v => v.CommentId == request.CommentId);
                comment.IsDeleteable(votesSum, user);
                _commentRepository.Delete(comment);
                return Unit.Value;
            }
        }
    }
}
