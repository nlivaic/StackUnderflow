﻿using AutoMapper;
using MediatR;
using StackUnderflow.Application.Users;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Questions.Commands
{
    public class CreateQuestionCommand : IRequest<QuestionGetModel>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
        public Guid CurrentUserId { get; set; }

        class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionGetModel>
        {
            private readonly ITagService _tagService;
            private readonly IUserService _userService;
            private readonly IQuestionRepository _questionRepository;
            private readonly IUserRepository _userRepository;
            private readonly BaseLimits _limits;
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public CreateQuestionCommandHandler(
                ITagService tagService,
                IUserService userService,
                IQuestionRepository questionRepository,
                IUserRepository userRepository,
                BaseLimits limits,
                IUnitOfWork uow,
                IMapper mapper)
            {
                _tagService = tagService;
                _userService = userService;
                _questionRepository = questionRepository;
                _userRepository = userRepository;
                _limits = limits;
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<QuestionGetModel> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
            {
                var tags = await _tagService.GetTagsAsync(request.TagIds);
                var user = await _userRepository.GetByIdAsync(request.CurrentUserId);
                var question = Question.Create(user, request.Title, request.Body, tags, _limits);
                await _questionRepository.AddAsync(question);
                await _uow.SaveAsync();
                var questionGetModel = _mapper.Map<QuestionGetModel>(question);
                questionGetModel.IsOwner = true;
                questionGetModel.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return questionGetModel;
            }
        }
    }
}
