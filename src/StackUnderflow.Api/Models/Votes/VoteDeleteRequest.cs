using AutoMapper;
using StackUnderflow.Api.Profiles;
using StackUnderflow.Application.Votes.Commands;
using System;

namespace StackUnderflow.Api.Models.Votes
{
    public class VoteDeleteRequest
    {
        public Guid VoteId { get; set; }

        public class VoteDeleteRequestProfile : Profile
        {
            public VoteDeleteRequestProfile()
            {
                CreateMap<VoteDeleteRequest, RevokeVoteCommand>()
                    .ForMember(dest => dest.CurrentUserId,
                        opts => opts.MapFrom<UserIdLoggedInResolver<VoteDeleteRequest, RevokeVoteCommand>>());
            }
        }
    }
}
