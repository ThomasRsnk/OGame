using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories;
using Djm.OGame.Web.Api.Dal.Repositories.Pin;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Repositories.Univers;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Services
{
    public interface IUnitOfWork
    {
        DbSet<T> DbSet<T>() where T : class;

        Task CommitAsync(CancellationToken cancellationToken = default (CancellationToken));
        IPinRepository Pins { get; }
        IUniversRepository Univers { get; }
        IPlayerRepository Players { get; }
    }
}