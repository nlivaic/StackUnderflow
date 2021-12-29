using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Application.Answers.Models;
using StackUnderflow.Application.Sorting;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Api.ResourceParameters
{
    public class AnswerResourceParameters : BaseSortable<AnswerGetViewModel>
    {
        public int PageSize { get; set; } = 3;
        [BindNever]
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public override IEnumerable<SortCriteria> SortBy { get; set; }

        public class AnswerResourceParametersProfile : Profile
        {
            public AnswerResourceParametersProfile()
            {
                CreateMap<AnswerResourceParameters, AnswerQueryParameters>()
                    .ForSortableMembers();
            }
        }
    }
}
