using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public interface IPinRepository : IRepository<Pin,int>
    {
        Task<List<Pin>> ToListForOwnerAsync(int ownedId, CancellationToken cancellationToken = default(CancellationToken));
    }
}