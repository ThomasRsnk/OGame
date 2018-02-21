using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class PlayersHttpResource : HttpResource, IPlayersResource
    {
        public PlayersHttpResource(IHttpClient httpClient) : base(httpClient, "players/")
        {
        }

        [DebuggerStepThrough]
        public Task<List<PlayerListItemBindingModel>> GetAllAsync(CancellationToken cancellationToken)
            => JsonToPocoAsync<List<PlayerListItemBindingModel>>(cancellationToken);

        [DebuggerStepThrough]
        public Task<PlayerDetailsBindingModel> GetDetailsAsync(int playerId, CancellationToken cancellationToken)
            => JsonToPocoAsync<PlayerDetailsBindingModel>(playerId.ToString(CultureInfo.InvariantCulture), cancellationToken);
    }

    
}
