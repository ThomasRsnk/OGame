using System.Collections.Generic;
using System.Linq;

namespace Djm.OGame.Web.Api.BindingModels.Pagination
{
    public static class EnumerablePageExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, Page page) => page.Paginate(query);

        public static PagedListViewModel<T> ToPagedListViewModel<T>(this List<T> query, Page page)
        {
            var count = query.Count();
            var items = query.Paginate(page).ToList();

            return PagedListViewModel.Create(items, count, page);
        }
    }
}