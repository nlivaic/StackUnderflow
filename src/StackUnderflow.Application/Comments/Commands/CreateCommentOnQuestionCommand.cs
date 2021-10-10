using AutoMapper;
using MediatR;
using StackUnderflow.Application.Users;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Comments.Commands
{
    public class CreateCommentOnQuestionCommand : IRequest<CommentForQuestionGetModel>
    {
        public Guid QuestionId { get; set; }
        public string Body { get; set; }
        public Guid CurrentUserId { get; set; }

        public class CreateCommentOnQuestionCommandHandler : IRequestHandler<CreateCommentOnQuestionCommand, CommentForQuestionGetModel>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userService;
            private readonly IUnitOfWork _uow;
            private readonly BaseLimits _limits;
            private readonly IMapper _mapper;

            public CreateCommentOnQuestionCommandHandler(
                IQuestionRepository questionRepository,
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                IUserService userService,
                IUnitOfWork unitOfWork,
                BaseLimits limits,
                IMapper mapper)
            {
                _questionRepository = questionRepository;
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _userService = userService;
                _uow = unitOfWork;
                _limits = limits;
                _mapper = mapper;
            }

            public async Task<CommentForQuestionGetModel> Handle(CreateCommentOnQuestionCommand request, CancellationToken cancellationToken)
            {
                var question = (await _questionRepository.GetQuestionWithCommentsAsync(request.QuestionId))
                    ?? throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                var user = await _userRepository.GetByIdAsync(request.CurrentUserId);
                var commentOrderNumber = question
                    .Comments
                    .Select(c => c.OrderNumber)
                    .OrderByDescending(c => c)
                    .FirstOrDefault() + 1;
                var comment = Comment.Create(user, request.Body, commentOrderNumber, _limits);
                question.Comment(comment);
                await _commentRepository.AddAsync(comment);
                await _uow.SaveAsync();
                var result = _mapper.Map<CommentForQuestionGetModel>(comment);
                result.IsOwner = result.UserId == request.CurrentUserId;
                result.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return result;
            }
        }
    }
}
