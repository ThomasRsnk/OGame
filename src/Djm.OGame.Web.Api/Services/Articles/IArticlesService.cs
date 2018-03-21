using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.ViewModels.Articles;



namespace Djm.OGame.Web.Api.Services.Articles
{
    public interface IArticlesService
    {
        Task<PagedListViewModel<ArticleViewModel>> GetListAsync(Page page,CancellationToken cancellation = default (CancellationToken));

        Task PublishAsync(ArticleCreateViewModel bindingModel,CancellationToken cancellation = default(CancellationToken));

        Task EditAsync<TModel>(ClaimsPrincipal user, int articleId, TModel model, CancellationToken cancellation = default(CancellationToken));

        Task<ArticleDetailsViewModel> GetAsync(int articleId, CancellationToken cancellation = default(CancellationToken));

        Task DeleteAsync(ClaimsPrincipal user, int articleId, CancellationToken cancellation = default(CancellationToken));

        Task<DateTime> GetLastEditionDateAsync(CancellationToken cancellation = default(CancellationToken));
    }
}