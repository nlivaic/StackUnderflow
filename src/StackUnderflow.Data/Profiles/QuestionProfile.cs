using System;
using System.Linq;
using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionGetModel>()
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => ((DateTime)src.CreatedOn).ToString("yyyy-MM-dd hh:mm:ss")));

            CreateMap<Question, QuestionSummaryGetModel>()
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => ((DateTime)src.CreatedOn).ToString("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.Answers,
                    opts => opts.MapFrom(src => src.Answers.Count()));
        }
    }
}
