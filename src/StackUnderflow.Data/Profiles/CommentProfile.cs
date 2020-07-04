using System;
using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentGetModel>()
                .ForMember(dest => dest.Username,
                opts => opts.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.CreatedOn,
                opts => opts.MapFrom(src => ((DateTime)src.CreatedOn).ToString("yyyy-MM-dd hh:mm:ss")));
        }
    }
}
