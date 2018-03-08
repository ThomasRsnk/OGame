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

        public async Task<Entities.Player> FirstOrDefaultAsync(int universeId, int playerId, CancellationToken cancellation )
        {
            return await DbSet.FirstOrDefaultAsync(p => p.UniverseId == universeId && p.Id == playerId,cancellation);
        }

        public async Task<Entities.Player> FirstOrDefaultAsync(string login, CancellationToken cancellation)
        {
            return await DbSet.FirstOrDefaultAsync(p => p.Login == login, cancellation);
        }

        public async Task<List<Entities.Player>> ToListAsync(int universeId, CancellationToken cancellation)
        {
            return await DbSet.Where(p => p.UniverseId == universeId).ToListAsync(cancellation);
        }
    }
}