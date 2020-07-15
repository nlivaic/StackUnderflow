using System.ComponentModel;
using StackUnderflow.API.Helpers;

namespace StackUnderflow.API.Services.Sorting
{
    [TypeConverter(typeof(SortingDirectionDtoConverter))]
    public class SortCriteriaDto
    {
        public string SortByCriteria { get; set; }
        public SortDirectionDto SortDirection { get; set; }

        public override string ToString() => $"{SortByCriteria} {SortDirection.ToString().ToLower()}";
    }
}
