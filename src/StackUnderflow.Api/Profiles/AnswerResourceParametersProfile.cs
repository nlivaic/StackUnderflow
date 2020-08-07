using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.QueryParameters;

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
