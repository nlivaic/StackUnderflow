using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StackUnderflow.Application.Votes.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Events;
using StackUnderflow.Core.Interfaces;

namespace StackUnderflow.Application.Votes.Commands
{
    public class CastVoteCommand : IRequest<VoteGetModel>
    {
        public Guid CurrentUserId { get; set; }
        public Guid TargetId { get; set; }
        public VoteTargetEnum VoteTarget { get; set; }
        public VoteTypeEnum VoteType { get; set; }

        private class CastVoteCommandHandler : IRequestHandler<CastVoteCommand, VoteGetModel>
        {
            private readonly IVoteService _voteService;
            private readonly IVoteRepository _voteRepository;
            private readonly IEventRegister _eventRegister;
            private readonly IMapper _mapper;

            public CastVoteCommandHandler(
                IVoteService voteService,
                IVoteRepository voteRepository,
                IEventRegister eventRegister,
                IMapper mapper)
            {
                _voteService = voteService;
                _voteRepository = voteRepository;
                _eventRegister = eventRegister;
                _mapper = mapper;
            }

            public async Task<VoteGetModel> Handle(CastVoteCommand request, CancellationToken cancellationToken)
            {
                var matchedVote = await _voteRepository
                    .GetSingleAsync(v => v.UserId == request.CurrentUserId
                        && (v.QuestionId == null || v.QuestionId == request.TargetId)
                        && (v.AnswerId == null || v.AnswerId == request.TargetId)
                        && (v.CommentId == null || v.CommentId == request.TargetId));
                if (matchedVote != null)
                {
                    throw new BusinessException($"User already voted on {request.VoteTarget} on '{matchedVote.CreatedOn}'.");
                }
                var target = await _voteService.GetVoteableFromRepositoryAsync(request.VoteTarget, request.TargetId);
                if (target == null)
                {
                    throw new EntityNotFoundException(request.VoteTarget.ToString(), request.TargetId);
                }
                var vote = Vote.CreateVote(request.CurrentUserId, target, request.VoteType);
                target.ApplyVote(vote);
                await _voteService.ChangeCachedVotesSumAfterVoteCast(vote);
                _eventRegister.RegisterEvent<IVoteCast>(new { UserId = vote.UserId, VoteType = vote.VoteType });
                return _mapper.Map<VoteGetModel>(vote);
            }
        }
    }
}
