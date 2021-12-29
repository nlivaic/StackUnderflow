using System.Collections.Generic;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Tests.Helpers
{
    public class ResourceParameters2 
        : BaseSortable<MappingSourceModel2>
    {
        public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
