﻿using AutoMapper;
using StackUnderflow.Api.Profiles;
using StackUnderflow.Core.Models;
using System;
using static StackUnderflow.Core.Entities.Vote;

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
                CreateMap<VoteCreateRequest, VoteCreateModel>()
                    .ForMember(dest => dest.UserId,
                        opts => opts.MapFrom<UserIdResolver<VoteCreateRequest, VoteCreateModel>>());
            }
        }
    }
}
