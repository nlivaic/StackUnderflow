using System.Collections.Generic;

namespace StackUnderflow.Application.Sorting.Models
{
    public interface ISortable
    {
        IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
