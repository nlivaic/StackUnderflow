using AutoMapper;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Models;
using StackUnderflow.Api.ResourceParameters;
using StackUnderflow.Core.QueryParameters;

namespace StackUnderflow.Api.Profiles
{
    public class QuestionSummaryResourceParametersProfile : Profile
    {
        public QuestionSummaryResourceParametersProfile()
        {
            CreateMap<QuestionSummaryResourceParameters, QuestionQueryParameters>()
                .ForSortableMembers<QuestionSummaryGetViewModel, QuestionSummaryGetModel, QuestionSummaryResourceParameters, QuestionQueryParameters>();
        }
    }
}
