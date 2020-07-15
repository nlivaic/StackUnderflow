using AutoMapper;

namespace StackUnderflow.Api.Profiles
{
    public static class SortableMapper
    {
        public static void ForSortableMembers<TResource, TEntity, TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : StackUnderflow.Api.ResourceParameters.ISortable
            where TDestination : StackUnderflow.Core.QueryParameters.ISortable
        {
            mapping
                .ForMember(dest => dest.SortBy,
                        opts => opts.MapFrom<SortCriteriaResolver<TResource, TEntity, TSource, TDestination>>());
        }
    }
}