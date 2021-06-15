using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Common.Paging;

namespace StackUnderflow.Data.QueryableExtensions
{
    public static class Paging
    {
        /// <summary>
        /// Project list from `TSource` to `TTarget` using AutoMapper.
        /// </summary>
        public static async Task<PagedList<TTarget>> ApplyPagingAsync<TSource, TTarget>(
            this IQueryable<TSource> query,
            IMapper _mapper,
            int pageNumber,
            int pageSize)
        {
            var totalItems = query.Count();
            var elements = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TTarget>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return new PagedList<TTarget>(elements, pageNumber, totalItems, pageSize);
        }

        /// <summary>
        /// Project list from `TSource` to `TTarget`.
        /// Provide projection delegate.
        /// </summary>
        public static async Task<PagedList<TTarget>> ApplyPagingAsync<TSource, TTarget>(
            this IQueryable<TSource> query,
            Expression<Func<TSource, TTarget>> projection,
            int pageNumber,
            int pageSize)
        {
            var totalItems = query.Count();
            var elements = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(projection)
                .ToListAsync();
            return new PagedList<TTarget>(elements, pageNumber, totalItems, pageSize);
        }
    }
}
