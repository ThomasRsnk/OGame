using System;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories.Base;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Repositories.Article
{
    public class ArticleRepository : Repository<Entities.Article, int>, IArticleRepository
    {
        public ArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public override Task<Entities.Article> FindAsync(int id, CancellationToken cancellation = default(CancellationToken))
        {
            return DbSet
                .Include(a => a.Content)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellation);
        }

        public Task<DateTime> GetLastEditionDateAsync(CancellationToken cancellation)
        {
            return DbSet.MaxAsync(a => a.LastEdit, cancellationToken: cancellation);
        }
    }
}