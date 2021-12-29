using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Api.Models;
using StackUnderflow.Application.Sorting;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Api.ResourceParameters
{
    public class QuestionSummaryResourceParameters : BaseSortable<QuestionSummaryGetViewModel>
    {
        public int PageSize { get; set; } = 3;
        [BindNever]
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<Guid> Tags { get; set; } = new List<Guid>();
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<Guid> Users { get; set; }
        public string SearchQuery { get; set; }
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public override IEnumerable<SortCriteria> SortBy { get; set; }

        public class QuestionSummaryResourceParametersProfile : Profile
        {
            public QuestionSummaryResourceParametersProfile()
            {
                CreateMap<QuestionSummaryResourceParameters, QuestionQueryParameters>()
                    .ForSortableMembers();
            }
        }
    }
}
