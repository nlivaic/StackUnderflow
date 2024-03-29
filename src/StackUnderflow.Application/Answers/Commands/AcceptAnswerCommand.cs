﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.WorkerServices.Users;

namespace StackUnderflow.Application.Answers.Commands
{
    public class AcceptAnswerCommand : IRequest<AnswerGetModel>
    {
        public Guid CurrentUserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }

        private class AcceptAnswerCommandHandler : IRequestHandler<AcceptAnswerCommand, AnswerGetModel>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly IAnswerRepository _answerRepository;
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public AcceptAnswerCommandHandler(
                IQuestionRepository questionRepository,
                IAnswerRepository answerRepository,
                IUserService userService,
                IMapper mapper)
            {
                _questionRepository = questionRepository;
                _answerRepository = answerRepository;
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<AnswerGetModel> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.GetQuestionWithAnswerAsync(request.QuestionId, request.AnswerId);
                var answer = question.Answers.SingleOrDefault()
                    ?? throw new EntityNotFoundException(nameof(Answer), request.AnswerId);
                question.AcceptAnswer(answer, request.CurrentUserId);
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
