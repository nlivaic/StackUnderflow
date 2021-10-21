using AutoMapper;
using MediatR;
using StackUnderflow.WorkerServices.Users;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackUnderflow.Application.Comments.Models;

namespace StackUnderflow.Application.Comments.Commands
{
    public class CreateCommentOnAnswerCommand : IRequest<CommentForAnswerGetModel>
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string Body { get; set; }
        public Guid CurrentUserId { get; set; }

        class CreateCommentOnAnswerCommandHandler : IRequestHandler<CreateCommentOnAnswerCommand, CommentForAnswerGetModel>
        {
            private readonly IAnswerRepository _answerRepository;
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userService;
            private readonly IUnitOfWork _uow;
            private readonly BaseLimits _limits;
            private readonly IMapper _mapper;

            public CreateCommentOnAnswerCommandHandler(
                IAnswerRepository answerRepository,
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                IUserService userService,
                IUnitOfWork unitOfWork,
                BaseLimits limits,
                IMapper mapper)
            {
                _answerRepository = answerRepository;
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _userService = userService;
                _uow = unitOfWork;
                _limits = limits;
                _mapper = mapper;
            }

            public async Task<CommentForAnswerGetModel> Handle(CreateCommentOnAnswerCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.CurrentUserId);
                var answer = await _answerRepository.GetAnswerWithCommentsAsync(request.QuestionId, request.AnswerId);
                if (answer == null)
                {
                    throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                }
                var commentOrderNumber = answer
                    .Comments
                    .Select(c => c.OrderNumber)
                    .OrderByDescending(c => c)
                    .FirstOrDefault() + 1;
                var comment = Comment.Create(user, request.Body, commentOrderNumber, _limits);
                answer.Comment(comment);
                await _commentRepository.AddAsync(comment);
                var result = _mapper.Map<CommentForAnswerGetModel>(comment);
                result.IsOwner = result.UserId == request.CurrentUserId;
                result.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return result;
            }
        }
    }
}
