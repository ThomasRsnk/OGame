using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public class Repository<TEntity,TKey> where TEntity : class
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            DbSet = unitOfWork.DbSet<TEntity>();
        }

        internal DbSet<TEntity> DbSet { get; }

        public virtual async Task<TEntity> FindAsync(TKey id, CancellationToken cancellation = default(CancellationToken))
        {
            return await DbSet.FindAsync(new object[]{ id },cancellation);
        }

        public virtual async Task InsertAsync(TEntity entity,CancellationToken cancellation = default(CancellationToken))
        {
            await DbSet.AddAsync(entity, cancellation);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellation = default(CancellationToken))
        {
            var entity = await DbSet.FindAsync(new object[] { id }, cancellation);
            DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}