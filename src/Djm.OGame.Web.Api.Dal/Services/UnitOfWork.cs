using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories;
using Djm.OGame.Web.Api.Dal.Repositories.Pin;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Repositories.Univers;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Db { get; }
        public IPinRepository Pins { get; }
        public IUniversRepository Univers { get; }
        public IPlayerRepository Players { get; }

        public UnitOfWork(OGameContext dbContext)
        {
            Db = dbContext;
            Pins = new PinRepository(this);
            Univers = new UniversRepository(this);
            Players = new PlayerRepository(this);
        }

        public DbSet<T> DbSet<T>() where T : class
        {
            return Db.Set<T>();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken)) 
            => Db.SaveChangesAsync(cancellationToken);
    }
}