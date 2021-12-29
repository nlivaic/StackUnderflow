using AutoMapper;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public static class SortableMapper
    {
        public static void ForSortableMembers<TResource, TEntity, TSourceParameters, TTargetParameters>(this IMappingExpression<TSourceParameters, TTargetParameters> mapping)
            where TSourceParameters : ISortable
            where TTargetParameters : ISortable
        {
            mapping
                .ForMember(
                    dest => dest.SortBy,
                    opts => opts.MapFrom<SortCriteriaResolver<TResource, TEntity, TSourceParameters, TTargetParameters>>());
        }
    }
}
