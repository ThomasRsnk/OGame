using OGame.Client.Models;
using OGame.Client.Providers.Cache;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Log
{
    internal class PlayerLogProvider : LogProvider<int,Player>, IPlayerProvider
    {
        public PlayerLogProvider(string prefix, IEntityProvider<int, Player> subProvider) : base(prefix, subProvider)
        { }
    }
}