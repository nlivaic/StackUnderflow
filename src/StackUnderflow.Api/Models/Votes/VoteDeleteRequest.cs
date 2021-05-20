using AutoMapper;
using StackUnderflow.Api.Profiles;
using StackUnderflow.Core.Models;
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
                CreateMap<VoteDeleteRequest, VoteRevokeModel>()
                    .ForMember(dest => dest.UserId,
                        opts => opts.MapFrom<UserIdLoggedInResolver<VoteDeleteRequest, VoteRevokeModel>>());
            }
        }
    }
}
