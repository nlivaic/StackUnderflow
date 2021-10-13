using System.Collections.Generic;

namespace StackUnderflow.Core.Sorting.Models
{
    public interface ISortable
    {
        IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
