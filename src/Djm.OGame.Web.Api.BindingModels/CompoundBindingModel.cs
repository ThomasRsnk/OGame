using Djm.OGame.Web.Api.BindingModels.Account;
using Djm.OGame.Web.Api.BindingModels.Articles;
using Djm.OGame.Web.Api.BindingModels.Pagination;

namespace Djm.OGame.Web.Api.BindingModels
{
    public class CompoundBindingModel
    {
        public PagedListViewModel<ArticleListItemBindingModel> Pagination { get; set; }
        public RegisterBindingModel Registration { get; set; }
        public LoginBindingModel Connection { get; set; }
        public ArticleDetailsBindingModel Article { get; set; }
        public CreateArticleBindingModel CreateArticle { get; set; }
    }
}