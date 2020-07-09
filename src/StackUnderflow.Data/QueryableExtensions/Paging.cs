using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Collections;

namespace StackUnderflow.Data.QueryableExtensions
{
    public static class Paging
    {
        public static async Task<PagedList<TTarget>> ApplyPagingAsync<TSource, TTarget>(this IQueryable<TSource> query, IMapper _mapper, int pageNumber, int pageSize)
        {
            var totalItems = query.Count();
            var elements = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TTarget>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new PagedList<TTarget>(elements, pageNumber, totalItems, pageSize);
        }
    }
}
