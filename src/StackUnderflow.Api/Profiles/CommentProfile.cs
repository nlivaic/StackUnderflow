using System;
using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentForQuestionGetModel, CommentForQuestionGetViewModel>()
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
            CreateMap<CommentForAnswerGetModel, CommentForAnswerGetViewModel>()
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
            CreateMap<CommentCreateRequest, CommentOnQuestionCreateModel>();
            CreateMap<CommentCreateRequest, CommentOnAnswerCreateModel>();
            CreateMap<UpdateCommentRequest, CommentEditModel>();
        }
    }
}
