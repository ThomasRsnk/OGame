using System;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories.Base;

namespace Djm.OGame.Web.Api.Dal.Repositories.Article
{
    public interface IArticleRepository : IRepository<Entities.Article, int>
    {
        Task<DateTime> GetLastEditionDateAsync(CancellationToken cancellation);
    }
}