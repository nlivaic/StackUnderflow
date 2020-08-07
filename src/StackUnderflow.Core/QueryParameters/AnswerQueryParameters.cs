using System.Collections.Generic;
using StackUnderflow.Core.QueryParameters.Sorting;

namespace StackUnderflow.Core.QueryParameters
{
    public class AnswerQueryParameters : ISortable
    {
        public int PageSize { get; set; } = 3;
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
