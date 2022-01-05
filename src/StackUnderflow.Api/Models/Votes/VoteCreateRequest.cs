using System;
using AutoMapper;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Application.Votes.Commands;
using StackUnderflow.Core.Enums;

namespace StackUnderflow.Api.Models.Votes
{
    public class VoteCreateRequest
    {
        public Guid TargetId { get; set; }
        public VoteTargetEnum VoteTarget { get; set; }
        public VoteTypeEnum VoteType { get; set; }

        public class VoteCreateRequestProfile : Profile
        {
            public VoteCreateRequestProfile()
            {
                CreateMap<VoteCreateRequest, CastVoteCommand>()
                    .ForMember(
                        dest => dest.CurrentUserId,
                        opts => opts.MapFrom<UserIdLoggedInResolver<VoteCreateRequest, CastVoteCommand>>());
            }
        }
    }
}
