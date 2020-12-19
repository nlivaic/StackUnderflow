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
            CreateMap<Question, QuestionGetModel>()
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Tags,
                    opts => opts.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)));

            CreateMap<Question, QuestionSummaryGetModel>()
                .ForMember(dest => dest.ShortBody,
                    opts => opts.MapFrom(src => src.Body.Substring(0, 50) + "..."))
                .ForMember(dest => dest.Username,
                    opts => opts.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Answers,
                    opts => opts.MapFrom(src => src.Answers.Count()))
                .ForMember(dest => dest.Tags,
                    opts => opts.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)));
        }
    }
}
