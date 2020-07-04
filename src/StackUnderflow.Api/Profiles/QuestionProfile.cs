using System.Linq;
using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionGetModel, QuestionGetViewModel>();
            CreateMap<QuestionSummaryGetModel, QuestionSummaryGetViewModel>()
                .ForMember(dest => dest.Tags,
                    opts => opts.MapFrom(src => src.Tags.Select(t => t.Name)));
        }
    }
}
