using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Players;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IPlayersResource
    {
        Task<PagedListViewModel<PlayerListItemBindingModel>> GetAllAsync(Page page,CancellationToken cancellationToken = default(CancellationToken));
        Task<PlayerDetailsBindingModel> GetDetailsAsync(int playerId, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Connect(string playerName, CancellationToken cancellationToken = default(CancellationToken));
    }
}