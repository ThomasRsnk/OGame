using System.Collections.Generic;
using OGame.Client.Models;

namespace OGame.Client.Providers.Web
{
    internal interface IPlanetProvider : IEntityProvider<int, Planet>
    {
        List<Planet> GetPlanetsForPlayer(int playerId);
    }
}