using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Djm.OGame.Web.Api.BindingModels.Pagination
{
    public static class PagedListViewModel
    {
        public static PagedListViewModel<TItem> Create<TItem>(List<TItem> items, int itemsCount, Page page)
            => Create(items, itemsCount, page.Size, page.Current);

        public static PagedListViewModel<TItem> Create<TItem>(List<TItem> items, int itemsCount, int pageLength, int currentPage)
        {
            return new PagedListViewModel<TItem>()
            {
                Items = items,

                CurrentPage = currentPage,
                PageSize = pageLength,
                TotalCount = itemsCount,
                TotalPages = (itemsCount % pageLength) != 0 ? (itemsCount / pageLength) + 1 : itemsCount / pageLength
            };
        }
    }

    public class PagedListViewModel<TItem> 
    {
        

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public List<TItem> Items { get; set; }

        public int Next
            => CurrentPage+1;

        public int Previous
            => CurrentPage-1;

        public bool HasPreviousPage
            => CurrentPage > 1;

        public bool HasNextPage
            => CurrentPage < TotalPages;


        public int Start { get; set; }
        public int End { get; set; }

        public void Paginate()
        {
            Start = CurrentPage - 5;
            End = CurrentPage + 4;

            if (Start <= 0)
            {
                Start = 1;
            }

            if (End > TotalPages)
            {
                End = TotalPages;

                if (End > 10)
                    Start = End - 9;
            }

           
        }
        
        
    }
}