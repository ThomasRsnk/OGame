using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories.Base;

namespace Djm.OGame.Web.Api.Dal.Repositories.Pin
{
    public interface IPinRepository : IRepository<Entities.Pin,int>
    {
        Task<List<Entities.Pin>> ToListForOwnerAsync(int ownedId, CancellationToken cancellationToken = default(CancellationToken));
    }
}