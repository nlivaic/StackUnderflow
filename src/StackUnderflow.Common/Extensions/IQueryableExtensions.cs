using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Linq;

namespace StackUnderflow.Common.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Provides an opportunity to project to different types.
        /// If `T` is an entity (inheriting from `BaseEntity<Guid>`) then pass through.
        /// If `T` is not an entity then map to `T˙ using AutoMapper.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="provider"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Projector<T>(this IQueryable query, IConfigurationProvider provider)
        {
            var type = typeof(T);
            while (type != typeof(object)) {
                if (type == typeof(Common.Base.BaseEntity<Guid>))
                {
                    return query.Cast<T>();
                }
                type = type.BaseType;
            }
            return query.ProjectTo<T>(provider);
        }
    }
}
