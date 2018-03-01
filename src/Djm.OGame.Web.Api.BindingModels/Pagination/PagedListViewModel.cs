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
    }
}