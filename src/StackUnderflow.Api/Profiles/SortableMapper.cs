using AutoMapper;

namespace StackUnderflow.Api.Profiles
{
    public static class SortableMapper
    {
        public static void ForSortableMembers<TResource, TEntity, TSourceParameters, TDestinationParameters>(this IMappingExpression<TSourceParameters, TDestinationParameters> mapping)
            where TSourceParameters : StackUnderflow.Api.ResourceParameters.ISortable
            where TDestinationParameters : StackUnderflow.Core.QueryParameters.ISortable
        {
            mapping
                .ForMember(dest => dest.SortBy,
                        opts => opts.MapFrom<SortCriteriaResolver<TResource, TEntity, TSourceParameters, TDestinationParameters>>());
        }
    }
}