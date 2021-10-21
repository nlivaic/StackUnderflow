using AutoMapper;
using MediatR;
using StackUnderflow.WorkerServices.Users;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using StackUnderflow.Application.Answers.Models;

namespace StackUnderflow.Application.Answers.Commands
{
    public class CreateAnswerCommand : IRequest<AnswerGetModel>
    {
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; }
        public Guid? CurrentUserId { get; set; }

        class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, AnswerGetModel>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly IAnswerRepository _answerRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userService;
            private readonly BaseLimits _limits;
            private readonly IMapper _mapper;

            public CreateAnswerCommandHandler(
                IQuestionRepository questionRepository,
                IAnswerRepository answerRepository,
                IUserRepository userRepository,
                IUserService userService,
                BaseLimits limits,
                IMapper mapper)
            {
                _questionRepository = questionRepository;
                _answerRepository = answerRepository;
                _userRepository = userRepository;
                _userService = userService;
                _limits = limits;
                _mapper = mapper;
            }

            public async Task<AnswerGetModel> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.GetQuestionWithAnswersAsync(request.QuestionId)
                    ?? throw new EntityNotFoundException(nameof(Question), request.QuestionId);

                if (!(await _questionRepository.ExistsAsync(request.QuestionId)))
                {
                    throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                }
                var user = await _userRepository.GetByIdAsync(request.UserId);
                var answer = Answer.Create(user, request.Body, question, _limits);
                question.Answer(answer);
                await _answerRepository.AddAsync(answer);
                //await _uow.SaveAsync();
                var result = _mapper.Map<AnswerGetModel>(answer);
                result.IsOwner = result.UserId == request.CurrentUserId;
                result.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                // @nl: Raise an event! Message must be sent to the inbox.
                return result;
            }
        }
    }
}
