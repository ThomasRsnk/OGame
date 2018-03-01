using OGame.Client.Models;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Cache
{
    internal class CachePlayerProvider : ProviderCache<int, Player>, IPlayerProvider
    {
        public CachePlayerProvider(IEntityProvider<int, Player> subProvider) : base(subProvider)
        {
        }
    }
}