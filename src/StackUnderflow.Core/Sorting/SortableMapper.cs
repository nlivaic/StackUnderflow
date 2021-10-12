using AutoMapper;
using StackUnderflow.WorkerServices.PointServices.Sorting.Models;

namespace StackUnderflow.WorkerServices.PointServices.Sorting
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
