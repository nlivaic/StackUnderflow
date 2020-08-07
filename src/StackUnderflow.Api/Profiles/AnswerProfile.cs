using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<AnswerGetModel, AnswerGetViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    opts => opts.MapFrom(
                        src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
        }
    }
}
