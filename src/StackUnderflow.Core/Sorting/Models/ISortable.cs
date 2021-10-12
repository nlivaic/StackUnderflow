using System.Collections.Generic;

namespace StackUnderflow.WorkerServices.PointServices.Sorting.Models
{
    public interface ISortable
    {
        IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
