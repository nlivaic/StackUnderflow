﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StackUnderflow.Application.Comments.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.WorkerServices.Users;

namespace StackUnderflow.Application.Comments.Queries
{
    public class GetCommentForAnswerQuery : IRequest<CommentForAnswerGetModel>
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid CommentId { get; set; }
        public Guid? CurrentUserId { get; set; }

        private class GetCommentForAnswerQueryHandler : IRequestHandler<GetCommentForAnswerQuery, CommentForAnswerGetModel>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserService _userService;

            public GetCommentForAnswerQueryHandler(
                ICommentRepository commentRepository,
                IUserService userService)
            {
                _commentRepository = commentRepository;
                _userService = userService;
            }

            public async Task<CommentForAnswerGetModel> Handle(GetCommentForAnswerQuery request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentForAnswerAsync(request.AnswerId, request.CommentId);
                if (comment == null || comment.QuestionId != request.QuestionId)
                {
                    throw new EntityNotFoundException(nameof(Comment), request.CommentId);
                }
                comment.IsOwner = comment.UserId == request.CurrentUserId;
                comment.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return comment;
            }
        }
    }
}
