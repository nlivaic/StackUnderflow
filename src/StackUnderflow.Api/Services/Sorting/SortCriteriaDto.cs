using System.ComponentModel;
using StackUnderflow.Api.Helpers;

namespace StackUnderflow.Api.Services.Sorting
{
    [TypeConverter(typeof(SortingDirectionDtoConverter))]
    public class SortCriteriaDto
    {
        public string SortByCriteria { get; set; }
        public SortDirectionDto SortDirection { get; set; }

        public override string ToString() => $"{SortByCriteria} {SortDirection.ToString().ToLower()}";
    }
}
