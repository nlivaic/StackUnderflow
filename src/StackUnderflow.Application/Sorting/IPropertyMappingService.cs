using System.Collections.Generic;

namespace StackUnderflow.Application.Sorting
{
    public interface IPropertyMappingService
    {
        PropertyMappingValue GetMapping<TSource, TTarget>(string sourcePropertyName);
        IEnumerable<PropertyMappingValue> GetMappings<TSource, TTarget>(params string[] sourcePropertyNames);
    }
}
