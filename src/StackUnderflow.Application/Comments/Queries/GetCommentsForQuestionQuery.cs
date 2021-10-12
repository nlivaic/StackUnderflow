using MediatR;
using StackUnderflow.WorkerServices.Users;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.Comments.Queries
{
    public class GetCommentsForQuestionQuery : IRequest<IEnumerable<CommentForQuestionGetModel>>
    {
        public Guid QuestionId { get; set; }
        public Guid? CurrentUserId { get; set; }

        class GetCommentsForQuestionQueryHandler : IRequestHandler<GetCommentsForQuestionQuery, IEnumerable<CommentForQuestionGetModel>>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly ICommentRepository _commentRepository;
            private readonly IUserService _userService;

            public GetCommentsForQuestionQueryHandler(
                IQuestionRepository questionRepository,
                ICommentRepository commentRepository,
                IUserService userService)
            {
                _questionRepository = questionRepository;
                _commentRepository = commentRepository;
                _userService = userService;
            }

            public async Task<IEnumerable<CommentForQuestionGetModel>> Handle(GetCommentsForQuestionQuery request, CancellationToken cancellationToken)
            {
                if (!(await _questionRepository.ExistsAsync(request.QuestionId)))
                {
                    throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                }
                var result = await _commentRepository.GetCommentsForQuestionAsync<CommentForQuestionGetModel>(request.QuestionId);
                foreach (var comment in result)
                {
                    comment.IsOwner = comment.UserId == request.CurrentUserId;
                    comment.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                }
                return result;
            }
        }
    }
}
