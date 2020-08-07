using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using StackUnderflow.Core.QueryParameters.Sorting;

namespace StackUnderflow.Data.QueryableExtensions
{
    public static class Sorting
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IEnumerable<SortCriteria> sortCriteria)
        // =>
        //     sortCriteria
        //         .Select((sortCriteria, index) => new { SortCriteria = sortCriteria, Index = index })
        //         .Aggregate(query, (q, sort) =>
        //             sort.Index == 0
        //                 ? q.OrderBy(sort.SortCriteria.ToString())
        //                 : ((IOrderedQueryable<T>)q).ThenBy(sort.SortCriteria.ToString()));
        {
            var q = query.OrderBy(sortCriteria.ToList()[0].ToString());
            // q = q.ThenBy(sortCriteria.ToList()[1].ToString());
            return q;
        }
    }
}
