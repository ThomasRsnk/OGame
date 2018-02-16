using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Players;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IPlayersResource
    {
        Task<List<PlayerListItemBindingModel>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<PlayerDetailsBindingModel> GetDetailsAsync(int playerId, CancellationToken cancellationToken = default(CancellationToken));
    }
}