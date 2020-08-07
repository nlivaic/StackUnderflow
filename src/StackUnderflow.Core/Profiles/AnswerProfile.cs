using AutoMapper;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Core.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerGetModel>()
                .ForMember(
                    dest => dest.Username,
                    opts => opts.MapFrom(
                        src => src.User.Username));
        }
    }
}
