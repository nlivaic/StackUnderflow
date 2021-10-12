using MediatR;
using StackUnderflow.WorkerServices.Users;
using StackUnderflow.WorkerServices.Votes;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.WorkerServices.Questions.Commands
{
    public class GetQuestionQuery : IRequest<QuestionGetModel>
    {
        public Guid QuestionId { get; set; }
        public Guid? CurrentUserId { get; set; }

        class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, QuestionGetModel>
        {
            private readonly IQuestionRepository _questionRepository;
            private readonly IUserService _userService;
            private readonly IVoteService _voteService;

            public GetQuestionQueryHandler(
                IQuestionRepository questionRepository,
                IUserService userService,
                IVoteService voteService)
            {
                _questionRepository = questionRepository;
                _userService = userService;
                _voteService = voteService;
            }

            public async Task<QuestionGetModel> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.GetQuestionWithUserAndTagsAsync(request.QuestionId, request.CurrentUserId);
                if (question == null)
                {
                    throw new EntityNotFoundException(nameof(Question), request.QuestionId);
                }
                if (question != null)
                {
                    question.VotesSum = await _voteService.GetVotesSumAsync(request.QuestionId, VoteTargetEnum.Question);
                }
                question.IsOwner = question.UserId == request.CurrentUserId;
                question.IsModerator = await _userService.IsModeratorAsync(request.CurrentUserId);
                return question;
            }
        }
    }
}
