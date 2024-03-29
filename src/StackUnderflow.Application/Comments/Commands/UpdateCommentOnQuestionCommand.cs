﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Application.Comments.Commands
{
    public class UpdateCommentOnQuestionCommand : IRequest<Unit>
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid CurrentUserId { get; set; }
        public Guid CommentId { get; set; }
        public string Body { get; set; }

        private class UpdateCommentOnQuestionCommandHandler : IRequestHandler<UpdateCommentOnQuestionCommand, Unit>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly ILimits _limits;

            public UpdateCommentOnQuestionCommandHandler(
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                ILimits limits)
            {
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _limits = limits;
            }

            public async Task<Unit> Handle(UpdateCommentOnQuestionCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentWithQuestionAsync(request.CommentId);
                if (comment == null || comment.ParentQuestionId != request.ParentQuestionId)
                {
                    throw new EntityNotFoundException(nameof(Comment), request.CommentId);
                }
                var user = await _userRepository.GetUser<User>(request.CurrentUserId);
                comment.Edit(user, request.Body, _limits);

                // @nl: raise an event?
                return Unit.Value;
            }
        }
    }
}
