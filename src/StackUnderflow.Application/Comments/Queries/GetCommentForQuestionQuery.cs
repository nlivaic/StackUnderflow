using System;
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
    public class GetCommentForQuestionQuery : IRequest<CommentForQuestionGetModel>
    {
        public Guid QuestionId { get; set; }
        public Guid CommentId { get; set; }
        public Guid? CurrentUserId { get; set; }

        private class GetCommentForQuestionQueryHandler : IRequestHandler<GetCommentForQuestionQuery, CommentForQuestionGetModel>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserService _userService;

            public GetCommentForQuestionQueryHandler(
                ICommentRepository commentRepository,
                IUserService userService)
            {
                _commentRepository = commentRepository;
                _userService = userService;
            }

            public async Task<CommentForQuestionGetModel> Handle(GetCommentForQuestionQuery request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentModelAsync(request.QuestionId, request.CommentId);
                if (comment == null)
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
