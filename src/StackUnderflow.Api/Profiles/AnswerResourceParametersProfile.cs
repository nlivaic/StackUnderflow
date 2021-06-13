using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Application.Services.Sorting;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Profiles
{
    public class AnswerResourceParametersProfile : Profile
    {
        public AnswerResourceParametersProfile()
        {
            CreateMap<AnswerResourceParameters, AnswerQueryParameters>()
                .ForSortableMembers<AnswerGetViewModel, AnswerGetModel, AnswerResourceParameters, AnswerQueryParameters>();
        }
    }
}
