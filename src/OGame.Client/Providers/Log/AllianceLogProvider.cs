using OGame.Client.Models;
using OGame.Client.Providers.Web;

namespace OGame.Client.Providers.Log
{
    internal class AllianceLogProvider : LogProvider<int, Alliance>, IAllianceProvider
    {
        public AllianceLogProvider(string prefix, IEntityProvider<int, Alliance> subProvider) : base(prefix, subProvider)
        {
        }
    }
}