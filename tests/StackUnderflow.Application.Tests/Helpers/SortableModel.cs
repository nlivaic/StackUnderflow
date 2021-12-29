using System.Collections.Generic;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Tests.Helpers
{
    public class SortableModel : ISortable
    {
        public IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
