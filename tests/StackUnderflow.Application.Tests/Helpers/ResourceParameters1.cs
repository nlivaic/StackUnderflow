using System.Collections.Generic;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Tests.Helpers
{
    public class ResourceParameters1
        : BaseSortable<MappingSourceModel1>
    {
        public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
