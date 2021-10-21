using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Votes.Commands
{
    public class RevokeVoteCommand : IRequest<Unit>
    {
        public Guid CurrentUserId { get; set; }
        public Guid VoteId { get; set; }

        class RevokeVoteCommandHandler : IRequestHandler<RevokeVoteCommand, Unit>
        {
            private readonly IVoteRepository _voteRepository;
            private readonly IVoteService _voteService;
            private readonly BaseLimits _limits;

            public RevokeVoteCommandHandler(
                IVoteRepository voteRepository,
                IVoteService voteService,
                BaseLimits limits)
            {
                _voteRepository = voteRepository;
                _voteService = voteService;
                _limits = limits;
            }

            public async Task<Unit> Handle(RevokeVoteCommand request, CancellationToken cancellationToken)
            {
                var vote = (await _voteRepository
                    .GetVoteWithTargetAsync(request.CurrentUserId, request.VoteId))
                    ?? throw new EntityNotFoundException(nameof(Vote), request.VoteId);
                if (vote.CreatedOn.Add(_limits.VoteEditDeadline) < DateTime.UtcNow)
                    throw new BusinessException($"Vote cannot be edited since more than '{_limits.VoteEditDeadline.Minutes}' minutes passed.");
                var voteable = _voteService.GetVoteable(vote);
                voteable.RevokeVote(vote, _limits);
                _voteRepository.Delete(vote);
                await _voteService.ChangeCachedVotesSumAfterVoteRevoked(vote);
                return Unit.Value;
            }
        }
    }
}
