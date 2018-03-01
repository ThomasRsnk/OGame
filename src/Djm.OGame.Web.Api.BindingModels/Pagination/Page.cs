using System.Collections.Generic;
using System.Linq;

namespace Djm.OGame.Web.Api.BindingModels.Pagination
{
    public class Page
    {
        public int Current { get; set; }
        public int Size { get; set; }

        public int FirstIndex => (Current - 1) * Size;
        
        public IEnumerable<T> Paginate<T>(IEnumerable<T> query)
        {
            return query.Skip(FirstIndex).Take(Size);
        }
    }
}