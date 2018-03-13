using Djm.OGame.Web.Api.Dal.Repositories.Base;
using Djm.OGame.Web.Api.Dal.Services;

namespace Djm.OGame.Web.Api.Dal.Repositories.Article
{
    public class ArticleContentRepository : Repository<Entities.ArticleContent, int>, IArticleContentRepository
    {
        public ArticleContentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}