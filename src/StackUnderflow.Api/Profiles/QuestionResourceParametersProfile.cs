using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Api.Profiles
{
    public class QuestionResourceParametersProfile : Profile
    {
        public QuestionResourceParametersProfile()
        {
            CreateMap<QuestionResourceParameters, QuestionQueryParameters>()
                .ForSortableMembers<QuestionSummaryGetViewModel, QuestionSummaryGetModel, QuestionResourceParameters, QuestionQueryParameters>();
        }
    }
}
