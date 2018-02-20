using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Djm.OGame.Web.Api.Dal.Models;

namespace Djm.OGame.Web.Api.Dal
{
    public interface IOgameDb
    {
        Pin Insert(Pin pin);
        void Delete(int pinId);

        List<Pin> ToList(int ownedId);
        Pin FirstOrDefault(int pinId);
    }

    public class OgameDb : IOgameDb
    {
        public OgameDb()
        {
            Ctx = new OGameContext();
        }

        private OGameContext Ctx { get; }

        public Pin Insert(Pin pin)
        {
            Ctx.Pins.Add(pin);
            Ctx.SaveChanges();
            return pin;

        }

        public void Delete(int pinId)
        {
            Ctx.Pins.Remove(FirstOrDefault(pinId));
        }

        public List<Pin> ToList(int ownedId)
        {
            return Ctx.Pins.Where(p => p.OwnerId == ownedId).Select(p=>p).ToList();
        }

        public Pin FirstOrDefault(int pinId)
        {
            return Ctx.Pins.FirstOrDefault(p => p.Id == pinId);
        }
    }
}