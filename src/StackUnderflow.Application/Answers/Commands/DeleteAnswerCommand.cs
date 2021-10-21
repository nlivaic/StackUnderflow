using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Answers.Commands
{
    public class DeleteAnswerCommand : IRequest<Unit>
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid? CurrentUserId { get; set; }

        class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, Unit>
        {
            private readonly IAnswerRepository _answerRepository;
            private readonly IRepository<Vote> _voteRepository;
            private readonly IRepository<Comment> _commentRepository;
            private readonly IUnitOfWork _uow;

            public DeleteAnswerCommandHandler(
                IAnswerRepository answerRepository,
                IRepository<Vote> voteRepository,
                IRepository<Comment> commentRepository,
                IUnitOfWork uow)
            {
                _answerRepository = answerRepository;
                _voteRepository = voteRepository;
                _commentRepository = commentRepository;
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
            {
                var answer = await
                    _answerRepository
                        .GetAnswerWithCommentsAndVotesAsync(request.QuestionId, request.AnswerId)
                    ?? throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                var votesSum = await
                    _voteRepository
                    .CountAsync(v => v.AnswerId == request.AnswerId);
                answer.IsDeleteable();
                _commentRepository.Delete(answer.Comments);
                _answerRepository.Delete(answer);
                return Unit.Value;
            }
        }
    }
}
