using System.Collections.Generic;

namespace StackUnderflow.Application.Services.Sorting
{
    public interface ISortable
    {
        IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
