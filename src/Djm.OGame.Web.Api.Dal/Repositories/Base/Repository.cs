using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Repositories.Base
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

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellation = default(CancellationToken))
        {
            var entity = await DbSet.FindAsync(new object[] { id }, cancellation);
            DbSet.Remove(entity);
        }

        public virtual void DeleteAsync(TEntity entity, CancellationToken cancellation = default(CancellationToken))
        {
           DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task<List<TEntity>> ToListAsync(CancellationToken cancellation = default(CancellationToken))
        {
            return await DbSet.ToListAsync(cancellation);
        }
    }
}