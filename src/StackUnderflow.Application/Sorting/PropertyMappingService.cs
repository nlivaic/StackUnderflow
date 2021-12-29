using System.Collections.Generic;
using System.Linq;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IEnumerable<IPropertyMapping> _propertyMappings;

        public PropertyMappingService(PropertyMappingOptions propertyMappingOptions)
        {
            _propertyMappings = propertyMappingOptions.PropertyMappings ?? new List<IPropertyMapping>();
        }

        /// <summary>
        /// Map source and destination properties. Returns a list of destination properties for each source property.
        /// One caveat: you cannot sort on destination properties that have additional aggregation methods applied to them
        /// in the AutoMapper profile (e.g. `Question.Answers.Count()` mapping to `QuestionSummariesGetViewModel.Answers`,
        /// because the resulting Linq query will cause EF Core to break.
        /// </summary>
        /// <typeparam name="TSource">Source Resource Parameters type.</typeparam>
        /// <typeparam name="TTarget">Target Query Parameters type.</typeparam>
        /// <param name="source">Instance of source object, with zero or more properties to source from.</param>
        /// <returns>A list of properties to execute the ordering on.</returns>
        public IEnumerable<SortCriteria> Resolve<TSource, TTarget>(ISortable source)
        {
            var sortCriterias = new List<SortCriteria>();
            foreach (var s in source.SortBy)
            {
                PropertyMappingValue targetMapping = null;
                try
                {
                    targetMapping = GetMapping<TSource, TTarget>(s.SortByCriteria);
                }
                catch (InvalidPropertyMappingException)
                {
                    // Skip erroneous mapping and move on to next sort criteria.
                    continue;
                }
                sortCriterias.AddRange(
                    targetMapping.TargetPropertyNames.Select(tpn =>
                        new SortCriteria
                        {
                            SortByCriteria = tpn,
                            SortDirection = targetMapping.Revert
                                ? s.SortDirection == SortDirection.Desc
                                    ? SortDirection.Asc
                                    : SortDirection.Desc
                                : s.SortDirection == SortDirection.Asc
                                    ? SortDirection.Asc
                                    : SortDirection.Desc
                        }));
            }
            return sortCriterias;
        }

        private PropertyMappingValue GetMapping<TSource, TTarget>(string sourcePropertyName)
        {
            var propertyMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TTarget>>()
                .FirstOrDefault()
                ?? throw new InvalidPropertyMappingException($"Unknown property mapping types: {typeof(TSource)}, {typeof(TTarget)}.");
            return propertyMapping.GetMapping(sourcePropertyName);
        }
    }
}
