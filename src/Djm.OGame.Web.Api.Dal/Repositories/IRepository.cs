using System.Threading;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public interface IRepository<TEntity, in TKey>
    {
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(TKey entityId, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<TEntity> FindAsync(TKey entityId, CancellationToken cancellationToken = default(CancellationToken));
        void Update(TEntity entity);
    }
}