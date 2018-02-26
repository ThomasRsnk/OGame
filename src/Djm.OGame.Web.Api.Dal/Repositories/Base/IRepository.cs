using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Dal.Repositories.Base
{
    public interface IRepository<TEntity, in TKey>
    {
        void Insert(TEntity entity);
        Task DeleteAsync(TKey entityId, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<TEntity> FindAsync(TKey entityId, CancellationToken cancellationToken = default(CancellationToken));
        void Update(TEntity entity);

        Task<List<TEntity>> ToListAsync(CancellationToken cancellation = default(CancellationToken));
    }
}