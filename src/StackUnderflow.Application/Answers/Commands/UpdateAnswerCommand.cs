using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.Answers.Commands
{
    public class UpdateAnswerCommand : IRequest<Unit>
    {
        public Guid CurrentUserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string Body { get; set; }

        class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, Unit>
        {
            private readonly IRepository<Answer> _answerRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _uow;
            private readonly BaseLimits _limits;

            public UpdateAnswerCommandHandler(
                IRepository<Answer> answerRepository,
                IUserRepository userRepository,
                IUnitOfWork uow,
                BaseLimits limits)

            {
                _answerRepository = answerRepository;
                _userRepository = userRepository;
                _uow = uow;
                _limits = limits;
            }

            public async Task<Unit> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
            {
                var answer = (await
                   _answerRepository
                       .GetSingleAsync(a =>
                           a.Id == request.AnswerId
                           && a.QuestionId == request.QuestionId))
                   ?? throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                var user = await _userRepository.GetUser<User>(request.CurrentUserId);
                answer.Edit(user, request.Body, _limits);
                await _uow.SaveAsync();
                return Unit.Value;
            }
        }
    }
}
