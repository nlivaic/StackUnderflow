using System.Collections.Generic;

namespace StackUnderflow.Application.Services.Sorting.Models
{
    public class AnswerQueryParameters : ISortable
    {
        public int PageSize { get; set; } = 3;
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
