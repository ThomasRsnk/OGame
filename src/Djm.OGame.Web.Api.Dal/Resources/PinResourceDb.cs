using System.Collections.Generic;
using System.Linq;
using Djm.OGame.Web.Api.Dal.Models;

namespace Djm.OGame.Web.Api.Dal.Resources
{
    public class PinResourceDb : IPinResource
    {
        public OGameContext Ctx { get; }

        public PinResourceDb(OGameContext ctx)
        {
            Ctx = ctx;
        }

        public Pin Insert(Pin pin)
        {
            Ctx.Pins.Add(pin);
            Ctx.SaveChanges();
            return pin;
        }

        public void Delete(int pinId)
        {
            var pin = Ctx.Pins.FirstOrDefault(p => p.Id == pinId);
            if (pin == null) return;
            Ctx.Pins.Remove(pin);
            Ctx.SaveChanges();
        }

        public List<Pin> ToList(int ownedId)
        {
            return Ctx.Pins.Where(p => p.OwnerId == ownedId).Select(p => p).ToList();
        }

        public Pin FirstOrDefault(int pinId)
        {
            return Ctx.Pins.FirstOrDefault(p => p.Id == pinId);
        }
    }
}