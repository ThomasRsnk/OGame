using OGame.Client.Models;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Cache
{
    internal class CacheAllianceProvider : ProviderCache<int, Alliance>, IAllianceProvider
    {
        public CacheAllianceProvider(IEntityProvider<int, Alliance> subProvider) : base(subProvider)
        {
        }
    }
}