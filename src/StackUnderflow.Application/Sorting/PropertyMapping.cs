using System;
using System.Collections.Generic;

namespace StackUnderflow.Application.Sorting
{
    public class PropertyMapping<TSource, TTarget> : IPropertyMapping
    {
        private Dictionary<string, PropertyMappingValue> _propertyMappingValues
            = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase);

        public PropertyMapping<TSource, TTarget> Add(bool areTargetPropertiesInReversedOrder, string sourcePropertyName, params string[] targetPropertyNames)
        {
            _propertyMappingValues.Add(
                sourcePropertyName,
                new PropertyMappingValue(sourcePropertyName, targetPropertyNames, areTargetPropertiesInReversedOrder));
            return this;
        }

        public PropertyMapping<TSource, TTarget> Add(string sourcePropertyName, params string[] targetPropertyNames)
        {
            return Add(false, sourcePropertyName, targetPropertyNames);
        }

        public PropertyMappingValue GetMapping(string sourcePropertyName)
        {
            _propertyMappingValues.TryGetValue(sourcePropertyName, out var propertyMappingValue);
            return
                propertyMappingValue
                ?? throw new InvalidPropertyMappingException($"Source property name '{sourcePropertyName}' for source type {typeof(TSource)} not mapped to target type {typeof(TTarget)}.");
        }

        public IEnumerable<PropertyMappingValue> GetMappings(params string[] sourcePropertyNames)
        {
            PropertyMappingValue propertyMappingValue = null;
            List<PropertyMappingValue> propertyMappingValues = new List<PropertyMappingValue>();
            Array.ForEach(sourcePropertyNames, sourcePropertyName =>
            {
                _propertyMappingValues.TryGetValue(sourcePropertyName, out propertyMappingValue);
                if (propertyMappingValue == null)
                {
                    throw new InvalidPropertyMappingException($"Source property name '{sourcePropertyName}' for source type {typeof(TSource)} not mapped to target type {typeof(TTarget)}.");
                }
                propertyMappingValues.Add(propertyMappingValue);
            });
            return propertyMappingValues;
        }
    }
}
