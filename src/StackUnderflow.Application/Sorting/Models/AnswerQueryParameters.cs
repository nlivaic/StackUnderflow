using System.Collections.Generic;
using StackUnderflow.Application.Answers.Models;

namespace StackUnderflow.Application.Sorting.Models
{
    public class AnswerQueryParameters : BaseSortable<AnswerGetModel>
    {
        private int _pageSize = 3;
        public int PageSize
        {
            get => _pageSize > MaximumPageSize ? MaximumPageSize : _pageSize;
            set => _pageSize = value;
        }
        public int MaximumPageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public override IEnumerable<SortCriteria> SortBy { get; set; }
    }
}
