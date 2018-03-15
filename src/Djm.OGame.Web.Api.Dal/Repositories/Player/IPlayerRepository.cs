using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories.Base;

namespace Djm.OGame.Web.Api.Dal.Repositories.Player
{
    public interface IPlayerRepository : IRepository<Entities.Player, int>
    {
        Task<Entities.Player> FirstOrDefaultAsync(int universeId, int playerId,
            CancellationToken cancellation = default(CancellationToken));

        Task<List<Entities.Player>> ToListAsync(int universeId,
            CancellationToken cancellation = default(CancellationToken));

        Task<Entities.Player> FirstOrDefaultAsync(string email,
            CancellationToken cancellation = default(CancellationToken));

        Task<List<Entities.Player>> ToListAsync(string adminOnly,
            CancellationToken cancellation = default(CancellationToken));
    }
}