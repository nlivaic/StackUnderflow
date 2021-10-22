using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Comments.Commands
{
    public class UpdateCommentOnAnswerCommand : IRequest<Unit>
    {
        public Guid? ParentQuestionId { get; set; }
        public Guid? ParentAnswerId { get; set; }
        public Guid CurrentUserId { get; set; }
        public Guid CommentId { get; set; }
        public string Body { get; set; }

        private class UpdateCommentOnAnswerCommandHandler : IRequestHandler<UpdateCommentOnAnswerCommand, Unit>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly BaseLimits _limits;

            public UpdateCommentOnAnswerCommandHandler(
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                BaseLimits limits)
            {
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _limits = limits;
            }

            public async Task<Unit> Handle(UpdateCommentOnAnswerCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentWithAnswerAsync(request.CommentId);
                if (comment == null
                    || comment.ParentAnswerId != request.ParentAnswerId
                    || comment.ParentAnswer.QuestionId != request.ParentQuestionId)
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
