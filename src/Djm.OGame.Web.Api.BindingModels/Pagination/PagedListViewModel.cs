using System.Collections.Generic;

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

        public int Start
        {
            get
            {
                if (CurrentPage == TotalPages)
                    return CurrentPage - 4;
                if (CurrentPage == TotalPages - 1)
                    return CurrentPage - 3;

                return CurrentPage - 2 > 1 ? CurrentPage - 2 : 2;
            }
        }
       

        public int End
        {
            get
            {
                if (CurrentPage < 3)
                    return 6;

                return CurrentPage + 2 < TotalPages ? CurrentPage + 3 : TotalPages;
                
            }
        }
        
    }
}