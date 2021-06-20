using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Comments.Commands
{
    public class UpdateCommentOnQuestionCommand : IRequest<Unit>
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid CurrentUserId { get; set; }
        public Guid CommentId { get; set; }
        public string Body { get; set; }

        public class UpdateCommentOnQuestionCommandHandler : IRequestHandler<UpdateCommentOnQuestionCommand, Unit>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly BaseLimits _limits;
            private readonly IUnitOfWork _uow;

            public UpdateCommentOnQuestionCommandHandler(
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                BaseLimits limits,
                IUnitOfWork unitOfWork)
            {
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _limits = limits;
                _uow = unitOfWork;
            }

            public async Task<Unit> Handle(UpdateCommentOnQuestionCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentWithQuestionAsync(request.CommentId);
                if (comment == null || comment.ParentQuestionId != request.ParentQuestionId)
                    throw new EntityNotFoundException(nameof(Comment), request.CommentId);
                var user = await _userRepository.GetUser<User>(request.CurrentUserId);
                comment.Edit(user, request.Body, _limits);
                await _uow.SaveAsync();
                // @nl: raise an event?
                return Unit.Value;
            }
        }
    }
}
