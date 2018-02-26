using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories.Base;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Repositories.Player
{
    public class PlayerRepository : Repository<Entities.Player, int>, IPlayerRepository
    {
        public PlayerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Entities.Player> FirstOrDefaultAsync(int universeId, int playerId,
            CancellationToken cancellation = default(CancellationToken))
        {
            return await DbSet.FirstOrDefaultAsync(p => p.UniverseId == universeId && p.Id == playerId,cancellation);
        }
    }
}