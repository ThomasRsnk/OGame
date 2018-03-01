using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
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
        public Task<PagedListViewModel<PlayerListItemBindingModel>> GetAllAsync(int page,int pageLength,CancellationToken cancellationToken)
            => JsonToPocoAsync<PagedListViewModel<PlayerListItemBindingModel>>("?page="+page+ "&pageLength=" + pageLength, cancellationToken);

        [DebuggerStepThrough]
        public Task<PlayerDetailsBindingModel> GetDetailsAsync(int playerId, CancellationToken cancellationToken)
            => JsonToPocoAsync<PlayerDetailsBindingModel>(playerId.ToString(CultureInfo.InvariantCulture), cancellationToken);
    }

    
}
