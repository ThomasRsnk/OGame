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

        public async Task<bool> Connect(string playerName,CancellationToken cancellationToken)
        {
            var response =  await HttpClient.GetAsync(BaseUrl + "connection?pseudo="+playerName, cancellationToken);

            return response.IsSuccessStatusCode;
        }

        [DebuggerStepThrough]
        public Task<List<PlayerListItemBindingModel>> GetAllAsync(int skip,int take,CancellationToken cancellationToken)
            => JsonToPocoAsync<List<PlayerListItemBindingModel>>("?skip="+skip+"&take="+take,cancellationToken);

        [DebuggerStepThrough]
        public Task<PlayerDetailsBindingModel> GetDetailsAsync(int playerId, CancellationToken cancellationToken)
            => JsonToPocoAsync<PlayerDetailsBindingModel>(playerId.ToString(CultureInfo.InvariantCulture), cancellationToken);
    }

    
}
