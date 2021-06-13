using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StackUnderflow.Api.Helpers;
using StackUnderflow.Application.Services.Sorting;

namespace StackUnderflow.Api.ResourceParameters
{
    public class QuestionSummaryResourceParameters : ISortable
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
        public IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
