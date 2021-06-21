using AutoMapper;
using MediatR;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models.Votes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackUnderflow.Application.Votes.Queries
{
    public class GetVoteQuery : IRequest<VoteGetModel>
    {
        public Guid VoteId { get; set; }

        class GetVoteQueryHandler : IRequestHandler<GetVoteQuery, VoteGetModel>
        {
            private readonly IVoteRepository _voteRepository;
            private readonly IMapper _mapper;

            public GetVoteQueryHandler(
                IVoteRepository voteRepository,
                IMapper mapper)
            {
                _voteRepository = voteRepository;
                _mapper = mapper;
            }

            public async Task<VoteGetModel> Handle(GetVoteQuery request, CancellationToken cancellationToken)
            {
                var vote = await _voteRepository.GetByIdAsync(request.VoteId);
                if (vote != null)
                {
                    throw new EntityNotFoundException(nameof(Vote), request.VoteId);
                }
                var voteModel = _mapper.Map<VoteGetModel>(vote);
                return voteModel;
            }
        }
    }
}
