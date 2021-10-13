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
    public class DeleteCommentOnAnswerCommand : IRequest<Unit>
    {
        public Guid ParentQuestionId { get; set; }
        public Guid ParentAnswerId { get; set; }
        public Guid CommentId { get; set; }
        public Guid CurrentUserId { get; set; }

        class DeleteCommentOnAnswerCommandHandler : IRequestHandler<DeleteCommentOnAnswerCommand, Unit>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IVoteRepository _voteRepository;
            private readonly IUnitOfWork _uow;

            public DeleteCommentOnAnswerCommandHandler(
                ICommentRepository commentRepository,
                IUserRepository userRepository,
                IVoteRepository voteRepository,
                IUnitOfWork unitOfWork)
            {
                _commentRepository = commentRepository;
                _userRepository = userRepository;
                _voteRepository = voteRepository;
                _uow = unitOfWork;
            }

            public async Task<Unit> Handle(DeleteCommentOnAnswerCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentWithAnswerAsync(request.CommentId);
                if (comment == null
                    || comment.ParentAnswerId != request.ParentAnswerId
                    || comment.ParentAnswer.QuestionId != request.ParentQuestionId)
                    throw new EntityNotFoundException(nameof(Comment), request.CommentId);
                var user = await _userRepository.GetUser<User>(request.CurrentUserId);
                var votesSum = await _voteRepository.CountAsync(v => v.CommentId == request.CommentId);
                comment.IsDeleteable(votesSum, user);
                _commentRepository.Delete(comment);
                await _uow.SaveAsync();
                return Unit.Value;
            }
        }
    }
}
