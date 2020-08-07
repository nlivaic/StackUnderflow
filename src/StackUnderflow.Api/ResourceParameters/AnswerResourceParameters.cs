using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using StackUnderflow.API.Helpers;
using StackUnderflow.API.Services.Sorting;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Api.ResourceParameters
{
    public class AnswerResourceParameters : ISortable
    {
        public int PageSize { get; set; } = 3;
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        [BindProperty(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<SortCriteriaDto> SortBy { get; set; } /*=
            new List<SortCriteriaDto> { new SortCriteriaDto { SortByCriteria = nameof(Answer.CreatedOn), SortDirection = SortDirectionDto.Desc } };*/
    }
}
