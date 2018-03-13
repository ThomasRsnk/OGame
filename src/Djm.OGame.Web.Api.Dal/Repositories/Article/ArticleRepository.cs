using Djm.OGame.Web.Api.Dal.Repositories.Base;
using Djm.OGame.Web.Api.Dal.Services;

namespace Djm.OGame.Web.Api.Dal.Repositories.Article
{
    public class ArticleRepository : Repository<Entities.Article, int>, IArticleRepository
    {
        public ArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }
    }
}