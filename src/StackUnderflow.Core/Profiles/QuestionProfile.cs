using System;
using System.Linq;
using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionWithUserAndAllCommentsModel>()
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => ((DateTime)src.CreatedOn).ToString("yyyy-MM-dd hh:mm:ss")));

            CreateMap<Question, QuestionSummaryModel>()
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => ((DateTime)src.CreatedOn).ToString("yyyy-MM-dd hh:mm:ss")))
                .ForMember(dest => dest.Answers,
                    opts => opts.MapFrom(src => src.Answers.Count()));
        }
    }
}
