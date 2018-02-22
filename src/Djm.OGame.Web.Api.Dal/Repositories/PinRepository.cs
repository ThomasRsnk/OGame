using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal.Repositories
{
    public class PinRepository : IPinRepository
    {
        public OGameContext Ctx { get; }

        public PinRepository(OGameContext ctx)
        {
            Ctx = ctx;
        }

        public async Task InsertAsync(Pin pin)
        {
            await Ctx.Pins.AddAsync(pin);
        }

        public async Task DeleteAsync(int pinId)
        {
            var pin = await Ctx.Pins.FirstOrDefaultAsync(p => p.Id == pinId);
            if (pin == null) return;
            Ctx.Pins.Remove(pin);
        }

        public async Task<List<Pin>> ToListForOwnerAsync(int ownedId)
        {
            return await Ctx.Pins.Where(p => p.OwnerId == ownedId).Select(p => p).ToListAsync();
        }

        public async Task<Pin> FirstOrDefaultAsync(int pinId)
        {
            return await Ctx.Pins.FirstOrDefaultAsync(p => p.Id == pinId);
        }

        public void Update(Pin pin)
        {
            Ctx.Pins.Update(pin);
        }

        public async Task SaveChangesAsync()
        {
            await Ctx.SaveChangesAsync();
        }
    }
}