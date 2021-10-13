using AutoMapper;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public static class SortableMapper
    {
        public static void ForSortableMembers<TResource, TEntity, TSourceParameters, TDestinationParameters>(this IMappingExpression<TSourceParameters, TDestinationParameters> mapping)
            where TSourceParameters : ISortable
            where TDestinationParameters : ISortable
        {
            mapping
                .ForMember(dest => dest.SortBy,
                        opts => opts.MapFrom<SortCriteriaResolver<TResource, TEntity, TSourceParameters, TDestinationParameters>>());
        }
    }
}
