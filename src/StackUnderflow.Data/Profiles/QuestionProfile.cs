using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionModel>()
                .ForMember(dest => dest.Username,
                opts => opts.MapFrom(src => src.User.Username));
        }
    }
}