using System.Collections.Generic;
using OGame.Client.Models;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Cache
{
    internal class CachePlanetProvider : ProviderCache<int, Planet>, IPlanetProvider
    {
        public CachePlanetProvider(IEntityProvider<int, Planet> subProvider) : base(subProvider)
        {
        }

        public List<Planet> GetPlanetsForPlayer(int playerId)
        {
            throw new System.NotImplementedException();
        }

    }
}