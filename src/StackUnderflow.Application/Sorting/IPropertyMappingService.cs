using System.Collections.Generic;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public interface IPropertyMappingService
    {
        IEnumerable<SortCriteria> Resolve<TSource, TTarget>(ISortable sortableSource);
    }
}
