using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Db { get; }
        
        public UnitOfWork(DbContext dbContext)
        {
            Db = dbContext;
        }

        public DbSet<T> DbSet<T>() where T : class
        {
            return Db.Set<T>();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken)) 
            => Db.SaveChangesAsync(cancellationToken);
    }
}