using System.Threading;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public interface IRepository<TEntity, in TKey>
    {
        void InsertAsync(TEntity entity);
        Task DeleteAsync(TKey entityId, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<TEntity> FindAsync(TKey entityId, CancellationToken cancellationToken = default(CancellationToken));
        void Update(TEntity entity);
    }
}