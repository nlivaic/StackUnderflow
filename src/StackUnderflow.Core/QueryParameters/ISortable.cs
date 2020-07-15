using System.Collections.Generic;
using StackUnderflow.Core.QueryParameters.Sorting;

namespace StackUnderflow.Core.QueryParameters
{
    public interface ISortable
    {
        IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
