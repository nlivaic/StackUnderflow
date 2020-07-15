using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using StackUnderflow.Core.QueryParameters.Sorting;

namespace StackUnderflow.Data.QueryableExtensions
{
    public static class Sorting
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IEnumerable<SortCriteria> sortCriteria) =>
            sortCriteria
                .Aggregate(query, (q, sort) =>
                    q.OrderBy(sort.ToString()));
    }
}
