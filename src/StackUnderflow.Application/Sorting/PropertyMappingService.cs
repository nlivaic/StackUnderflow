using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUnderflow.Application.Sorting
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IEnumerable<IPropertyMapping> _propertyMappings;

        public PropertyMappingService(PropertyMappingOptions propertyMappingOptions)
        {
            _propertyMappings = propertyMappingOptions.PropertyMappings ?? new List<IPropertyMapping>();
        }

        public PropertyMappingValue GetMapping<TSource, TTarget>(string sourcePropertyName)
        {
            var propertyMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TTarget>>()
                .FirstOrDefault()
                ?? throw new ArgumentException($"Unknown property mapping types: {typeof(TSource)}, {typeof(TTarget)}.");
            return propertyMapping.GetMapping(sourcePropertyName);
        }

        public IEnumerable<PropertyMappingValue> GetMappings<TSource, TTarget>(params string[] sourcePropertyNames)
        {
            var propertyMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TTarget>>()
                .FirstOrDefault()
                ?? throw new ArgumentException($"Unknown property mapping types: {typeof(TSource)}, {typeof(TTarget)}.");
            return propertyMapping.GetMappings(sourcePropertyNames);
        }
    }
}
