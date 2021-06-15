using System;
using System.Collections.Generic;

namespace StackUnderflow.Common.Paging
{
    public class PagedList<T>
    {
        public Paging Paging { get; }
        public int CurrentPage { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public bool HasNextPage { get; }
        public bool HasPreviousPage { get; }

        public List<T> Items { get; }

        public PagedList(List<T> items, int currentPage, int totalItems, int pageSize)
        {
            CurrentPage = currentPage;
            TotalItems = totalItems;
            TotalPages = Convert.ToInt32(Math.Ceiling((decimal)totalItems / pageSize));
            PageSize = pageSize;
            HasNextPage = CurrentPage < TotalPages;
            HasPreviousPage = CurrentPage > 1;
            Items = items;
            Paging = new Paging(
                CurrentPage,
                TotalPages,
                TotalItems,
                PageSize);
        }
    }
}
