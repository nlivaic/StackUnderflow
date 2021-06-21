using AutoMapper;
using StackUnderflow.Api.Profiles;
using StackUnderflow.Application.Votes.Commands;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Models;
using System;

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
                    .ForMember(dest => dest.CurrentUserId,
                        opts => opts.MapFrom<UserIdLoggedInResolver<VoteCreateRequest, CastVoteCommand>>());
            }
        }
    }
}
