using System;
using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentForQuestionGetModel>()
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username));
            CreateMap<Comment, CommentForAnswerGetModel>()
                .ForMember(dest => dest.AnswerId,
                    opts => opts.MapFrom(src => src.ParentAnswerId))
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username));
        }
    }
}
