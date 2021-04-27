using AutoMapper;
using StackUnderflow.Core.Entities;
using System;

namespace StackUnderflow.Core.Models.Votes
{
    public class VoteGetModel
    {
        public Guid VoteId { get; set; }
    }

    public class VoteGetModelProfile : Profile
    {
        public VoteGetModelProfile()
        {
            CreateMap<Vote, VoteGetModel>()
                .ForMember(dest => dest.VoteId,
                    opts => opts.MapFrom(src => src.Id));
        }
    }
}
