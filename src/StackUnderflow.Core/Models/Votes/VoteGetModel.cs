using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Enums;
using System;

namespace StackUnderflow.Core.Models.Votes
{
    public class VoteGetModel
    {
        public Guid Id { get; set; }
        public VoteTypeEnum VoteType { get; set; }
        public Guid TargetId { get; set; }
    }

    public class VoteGetModelProfile : Profile
    {
        public VoteGetModelProfile()
        {
            CreateMap<Vote, VoteGetModel>();
        }
    }
}
