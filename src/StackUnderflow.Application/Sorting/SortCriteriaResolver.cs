using System.Collections.Generic;
using AutoMapper;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public class SortCriteriaResolver<TResource, TEntity, TSource, TTarget>
        : IValueResolver<TSource, TTarget, IEnumerable<SortCriteria>>
        where TSource : ISortable
        where TTarget : ISortable
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public SortCriteriaResolver(IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService;
        }

        public IEnumerable<SortCriteria> Resolve(
            TSource source,
            TTarget target,
            IEnumerable<SortCriteria> destMember,
            ResolutionContext context) =>
            _propertyMappingService.Resolve<TResource, TEntity>(source);
    }
}
