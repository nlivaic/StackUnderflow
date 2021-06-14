using System.Collections.Generic;

namespace StackUnderflow.Application.Services.Sorting.Models
{
    public interface ISortable
    {
        IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
