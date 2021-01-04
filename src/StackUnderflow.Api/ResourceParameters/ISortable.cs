using System.Collections.Generic;
using StackUnderflow.Api.Services.Sorting;

namespace StackUnderflow.Api.ResourceParameters
{
    public interface ISortable
    {
        IEnumerable<SortCriteriaDto> SortBy { get; set; }
    }
}
