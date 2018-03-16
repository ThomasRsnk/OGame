using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Articles;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Services.OGame;

namespace Djm.OGame.Web.Api.Services.Articles
{
    public interface IArticlesService
    {
        Task<PagedListViewModel<ArticleListItemBindingModel>> GetListAsync(Page page,CancellationToken cancellation = default (CancellationToken));

        Task Publish(CreateArticleBindingModel bindingModel,CancellationToken cancellation = default(CancellationToken));

        Task Edit(ClaimsPrincipal user, int articleId,CreateArticleBindingModel bindingModel, CancellationToken cancellation = default(CancellationToken));

        Task<ArticleDetailsBindingModel> GetAsync(int articleId, CancellationToken cancellation = default(CancellationToken));

        Task Delete(ClaimsPrincipal user, int articleId, CancellationToken cancellation = default(CancellationToken));
    }
}