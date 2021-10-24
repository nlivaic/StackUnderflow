using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Application.Sorting.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Paging;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.WorkerServices.Users;

namespace StackUnderflow.Application.Answers.Queries
{
    public class GetAnswersQuery : IRequest<PagedList<AnswerGetModel>>
    {
        public GetAnswersQuery(
            Guid questionId,
            AnswerQueryParameters answerQueryParameters,
            Guid? currentUserId)
        {
            QuestionId = questionId;
            AnswerQueryParameters = answerQueryParameters;
            CurrentUserId = currentUserId;
        }

        public Guid QuestionId { get; private set; }
        public AnswerQueryParameters AnswerQueryParameters { get; private set; }
        public Guid? CurrentUserId { get; private set; }

        private class GetAnswersQueryHandler : IRequestHandler<GetAnswersQuery, PagedList<AnswerGetModel>>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly IAnswerRepository _answerRepository;
            private readonly IUserService _userService;

            public GetAnswersQueryHandler(
                IQuestionRepository questionRepository,
                IAnswerRepository answerRepository,
                IUserService userService)
            {
                _questionRepository = questionRepository;
                _answerRepository = answerRepository;
                _userService = userService;
            }

            public async Task<PagedList<AnswerGetModel>> Handle(GetAnswersQuery request, CancellationToken cancellationToken)
            {
                if (!(await _questionRepository.ExistsAsync(request.QuestionId)))
                {
                    throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                }
                var pagedAnswers = await _answerRepository.GetAnswersWithUserAsync(request.QuestionId, request.AnswerQueryParameters);
                pagedAnswers.Items.ForEach(async (a) =>
                {
                    a.IsOwner = a.UserId == request.CurrentUserId;
                    a.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                });
                return pagedAnswers;
            }
        }
    }
}
