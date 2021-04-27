using AutoMapper;
using StackUnderflow.Core.Models.Votes;
using System;

namespace StackUnderflow.Api.Models.Votes
{
    public class VoteGetViewModel
    {
        public Guid VoteId { get; set; }
    }

    public class VoteGetViewModelProfile : Profile
    {
        public VoteGetViewModelProfile()
        {
            CreateMap<VoteGetModel, VoteGetViewModel>();
        }
    }
}
