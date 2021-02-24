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
            CreateMap<QuestionGetModel, QuestionGetViewModel>()
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
            CreateMap<QuestionSummaryGetModel, QuestionSummaryGetViewModel>()
                .ForMember(dest => dest.Tags,
                    opts => opts.MapFrom(src => src.Tags.Select(t => t.Name)))
                .ForMember(dest => dest.CreatedOn,
                    opts => opts.MapFrom(src => src.CreatedOn.ToString("yyyy-MM-dd hh:mm:ss")));
            CreateMap<QuestionCreateRequest, QuestionCreateModel>();
            CreateMap<QuestionUpdateRequest, QuestionEditModel>();
        }
    }
}
