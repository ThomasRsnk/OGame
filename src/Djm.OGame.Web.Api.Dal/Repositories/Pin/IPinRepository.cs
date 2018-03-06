using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories.Base;

namespace Djm.OGame.Web.Api.Dal.Repositories.Pin
{
    public interface IPinRepository : IRepository<Entities.Pin,int>
    {
        Task<List<Entities.Pin>> ToListForOwnerAsync(int ownedId,int universeid, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<Entities.Pin>> ToListFortargetAsync(int targetId, int universeId,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}