using System.ComponentModel;

namespace StackUnderflow.Application.Sorting.Models
{
    [TypeConverter(typeof(SortingDirectionConverter))]
    public class SortCriteria
    {
        public string SortByCriteria { get; set; }
        public SortDirection SortDirection { get; set; }

        public override string ToString() => $"{SortByCriteria} {SortDirection.ToString().ToLower()}";
    }
}
