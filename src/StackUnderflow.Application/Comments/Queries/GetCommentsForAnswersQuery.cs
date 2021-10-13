using AutoMapper;
using MediatR;
using StackUnderflow.WorkerServices.Users;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StackUnderflow.Application.Comments.Models;

namespace StackUnderflow.WorkerServices.Comments.Queries
{
    public class GetCommentsForAnswersQuery : IRequest<IEnumerable<CommentForAnswerGetModel>>
    {
        public Guid QuestionId { get; set; }
        public IEnumerable<Guid> AnswerIds { get; set; }
        public Guid? CurrentUserId { get; set; }

        class GetCommentsForAnswersQueryHandler : IRequestHandler<GetCommentsForAnswersQuery, IEnumerable<CommentForAnswerGetModel>>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IQuestionRepository _questionRepository;
            private readonly IAnswerRepository _answerRepository;
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public GetCommentsForAnswersQueryHandler(
                ICommentRepository commentRepository,
                IQuestionRepository questionRepository,
                IAnswerRepository answerRepository,
                IUserService userService,
                IMapper mapper)
            {
                _commentRepository = commentRepository;
                _questionRepository = questionRepository;
                _answerRepository = answerRepository;
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CommentForAnswerGetModel>> Handle(GetCommentsForAnswersQuery request, CancellationToken cancellationToken)
            {
                if (!(await _questionRepository.ExistsAsync(request.QuestionId)) || !(await _answerRepository.ExistsAsync(request.AnswerIds)))
                {
                    throw new EntityNotFoundException(nameof(Answer), request.AnswerIds);
                }
                var comments = await _commentRepository.GetCommentsForAnswersAsync(request.AnswerIds);
                foreach (var comment in comments)
                {
                    comment.IsOwner = comment.UserId == request.CurrentUserId;
                    comment.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                }
                return comments;
            }
        }
    }
}
