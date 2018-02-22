using System.Collections.Generic;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public interface IPinRepository
    {
        Task InsertAsync(Pin pin);
        Task DeleteAsync(int pinId);

        Task<List<Pin>> ToListForOwnerAsync(int ownedId);
        Task<Pin> FirstOrDefaultAsync(int pinId);
        void Update(Pin pin);

        Task SaveChangesAsync();
    }
}