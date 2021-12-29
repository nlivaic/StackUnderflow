using System.Collections.Generic;
using AutoMapper;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public class SortCriteriaResolver<TSource, TTarget>
        : IValueResolver<TSource, TTarget, IEnumerable<SortCriteria>>
        where TSource : BaseSortable
        where TTarget : BaseSortable
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
            _propertyMappingService.Resolve(source, target);
    }
}
