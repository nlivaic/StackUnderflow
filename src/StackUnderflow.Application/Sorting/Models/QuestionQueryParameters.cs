using System;
using System.Collections.Generic;
using StackUnderflow.Application.Questions.Models;

namespace StackUnderflow.Application.Sorting.Models
{
    public class QuestionQueryParameters : BaseSortable<QuestionSummaryGetModel>
    {
        public int PageSize { get; set; } = 3;
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public List<Guid> Tags { get; set; }
        public List<Guid> Users { get; set; }
        public string SearchQuery { get; set; }
        public override IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
