using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StackUnderflow.Api.Exceptions;
using StackUnderflow.Api.Services.Sorting;
using StackUnderflow.Core.QueryParameters.Sorting;

namespace StackUnderflow.Api.Profiles
{
    public class SortCriteriaResolver<TResource, TEntity, TSource, TDestination>
        : IValueResolver<TSource, TDestination, IEnumerable<SortCriteria>>
        where TSource : StackUnderflow.Api.ResourceParameters.ISortable
        where TDestination : StackUnderflow.Core.QueryParameters.ISortable
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public SortCriteriaResolver(IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService;
        }

        public IEnumerable<SortCriteria> Resolve(TSource source, TDestination destination, IEnumerable<SortCriteria> destMember, ResolutionContext context)
        {
            List<SortCriteria> sortCriterias = new List<SortCriteria>();
            foreach (var s in source.SortBy)
            {
                PropertyMappingValue tp = null;
                try
                {
                    tp = _propertyMappingService.GetMapping<TResource, TEntity>(s.SortByCriteria);
                }
                catch (InvalidPropertyMappingException)
                {
                    // Skip erroneous mapping and move on to next sort criteria.
                    continue;
                }
                sortCriterias.AddRange(
                    tp.TargetPropertyNames.Select(tpn =>
                        new SortCriteria
                        {
                            SortByCriteria = tpn,
                            SortDirection = tp.Revert
                                ? s.SortDirection == SortDirectionDto.Desc
                                    ? SortDirection.Asc
                                    : SortDirection.Desc
                                : s.SortDirection == SortDirectionDto.Asc
                                    ? SortDirection.Asc
                                    : SortDirection.Desc
                        }));
            }
            return sortCriterias;
        }
    }
}
