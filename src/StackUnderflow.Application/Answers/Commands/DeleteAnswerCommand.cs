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
            private readonly IRepository<Answer> _answerRepository;
            private readonly IRepository<Vote> _voteRepository;
            private readonly IAnswerService _answerService;
            private readonly IUnitOfWork _uow;

            public DeleteAnswerCommandHandler(
                IRepository<Answer> answerRepository,
                IRepository<Vote> voteRepository,
                IAnswerService answerService,
                IUnitOfWork uow)
            {
                _answerRepository = answerRepository;
                _voteRepository = voteRepository;
                _answerService = answerService;
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
            {
                var answer = (await
                    _answerRepository
                        .GetSingleAsync(a =>
                            a.UserId == request.CurrentUserId
                            && a.Id == request.AnswerId
                            && a.QuestionId == request.QuestionId))
                    ?? throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                var votesSum = await
                    _voteRepository
                    .CountAsync(v => v.AnswerId == request.AnswerId);
                await _answerService.DeleteAnswerAsync(answer, votesSum);
                _answerRepository.Delete(answer);
                await _uow.SaveAsync();
                return Unit.Value;
            }
        }
    }
}
