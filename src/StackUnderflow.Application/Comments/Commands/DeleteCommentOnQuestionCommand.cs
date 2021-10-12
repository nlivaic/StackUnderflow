using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.Comments.Commands
{
    public class DeleteCommentOnQuestionCommand : IRequest<Unit>
    {
        public Guid ParentQuestionId { get; set; }
        public Guid CommentId { get; set; }
        public Guid CurrentUserId { get; set; }

        class DeleteCommentOnQuestionCommandHandler : IRequestHandler<DeleteCommentOnQuestionCommand, Unit>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IVoteRepository _voteRepository;
            private readonly IUnitOfWork _uow;

            public DeleteCommentOnQuestionCommandHandler(
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

            public async Task<Unit> Handle(DeleteCommentOnQuestionCommand request, CancellationToken cancellationToken)
            {
                var comment = await _commentRepository.GetCommentWithQuestionAsync(request.CommentId);
                if (comment == null
                    || comment.ParentQuestionId != request.ParentQuestionId)
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
