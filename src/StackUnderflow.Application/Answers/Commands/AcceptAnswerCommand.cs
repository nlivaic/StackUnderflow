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

namespace StackUnderflow.Application.Answers.Commands
{
    public class AcceptAnswerCommand : IRequest<AnswerGetModel>
    {
        public Guid CurrentUserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }

        public class AcceptAnswerCommanHandler : IRequestHandler<AcceptAnswerCommand, AnswerGetModel>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly IAnswerRepository _answerRepository;
            private readonly IUserService _userService;
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _uow;

            public AcceptAnswerCommanHandler(
                IQuestionRepository questionRepository,
                IAnswerRepository answerRepository,
                IUserService userService,
                IMapper mapper,
                IUnitOfWork uow)
            {
                _questionRepository = questionRepository;
                _answerRepository = answerRepository;
                _userService = userService;
                _mapper = mapper;
                _uow = uow;
            }

            public async Task<AnswerGetModel> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.GetQuestionWithAnswerAsync(request.QuestionId, request.AnswerId);
                var answer = question.Answers.SingleOrDefault()
                    ?? throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                question.AcceptAnswer(answer, request.CurrentUserId);
                await _uow.SaveAsync();
                var result = _mapper.Map<AnswerGetModel>(answer);
                result.IsOwner = answer.UserId == request.CurrentUserId;
                result.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return result;
                // @nl: calculate points.
                // @nl: raise an event. Message must be sent to answer owner's inbox.
            }
        }
    }
}
