using System;
using System.Data.Entity.Validation;
using Djm.OGame.Web.Api.Dal.Resources;

namespace Djm.OGame.Web.Api.Dal
{
    public interface IOgameDb
    {
        IPinResource Pins { get; }
    }
}