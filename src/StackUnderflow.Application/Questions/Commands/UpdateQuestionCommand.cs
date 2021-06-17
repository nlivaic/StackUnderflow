using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Questions.Commands
{
    public class UpdateQuestionCommand : IRequest<Unit>
    {
        public Guid CurrentUserId { get; set; }
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }

        public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Unit>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly ITagService _tagService;
            private readonly IUserRepository _userRepository;
            private readonly BaseLimits _limits;
            private readonly IUnitOfWork _uow;

            public UpdateQuestionCommandHandler(
                IQuestionRepository questionRepository,
                ITagService tagService,
                IUserRepository userRepository,
                BaseLimits limits,
                IUnitOfWork uow)
            {
                _questionRepository = questionRepository;
                _tagService = tagService;
                _userRepository = userRepository;
                _limits = limits;
                _uow = uow;
            }

            public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
            {
                var question = (await _questionRepository
                    .GetQuestionWithTagsAsync(request.QuestionId))
                    ?? throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                var tags = await _tagService.GetTagsAsync(request.TagIds);
                var user = await _userRepository.GetUser<User>(request.CurrentUserId);
                question.Edit(user, request.Title, request.Body, tags, _limits);
                await _uow.SaveAsync();
                return Unit.Value;
            }
        }
    }
}
