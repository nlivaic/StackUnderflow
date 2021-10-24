using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.WorkerServices.Users;

namespace StackUnderflow.Application.Answers.Queries
{
    public class GetAnswerQuery : IRequest<AnswerGetModel>
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public Guid? CurrentUserId { get; set; }

        private class GetAnswerQueryHandler : IRequestHandler<GetAnswerQuery, AnswerGetModel>
        {
            private readonly IAnswerRepository _answerRepository;
            private readonly IUserService _userService;

            public GetAnswerQueryHandler(
                IAnswerRepository answerRepository,
                IUserService userService)
            {
                _answerRepository = answerRepository;
                _userService = userService;
            }

            public async Task<AnswerGetModel> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
            {
                var result = await _answerRepository.GetAnswerWithUserAsync(request.QuestionId, request.AnswerId);
                if (result == null)
                {
                    throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                }
                result.IsOwner = result.UserId == request.CurrentUserId;
                result.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return result;
            }
        }
    }
}
