using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public class PinRepository : Repository<Pin, int>, IPinRepository
    {
        public PinRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        //méthodes spécifique au type de donnée

        public async Task<List<Pin>> ToListForOwnerAsync(int ownedId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DbSet.Where(p => p.OwnerId == ownedId).Select(p => p).ToListAsync(cancellationToken);
        }

    }
}