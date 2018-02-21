using System.Collections.Generic;
using Djm.OGame.Web.Api.Dal.Models;

namespace Djm.OGame.Web.Api.Dal.Resources
{
    public interface IPinResource
    {
        Pin Insert(Pin pin);
        void Delete(int pinId);

        List<Pin> ToList(int ownedId);
        Pin FirstOrDefault(int pinId);
    }
}