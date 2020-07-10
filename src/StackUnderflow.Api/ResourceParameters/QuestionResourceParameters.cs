using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.API.Helpers;

namespace StackUnderflow.Api.ResourceParameters
{
    public class QuestionResourceParameters
    {
        public int? PageSize { get; set; } = 3;
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<Guid> Tags { get; set; } = new List<Guid>();
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<Guid> Users { get; set; }
        public string SearchQuery { get; set; }
    }
}
