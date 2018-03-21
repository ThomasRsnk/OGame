using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Players;


namespace Djm.OGame.Web.Api.Services.OGame.Players
{
    public interface IPlayersService
    {
        Task<PagedListViewModel<PlayerListItemBindingModel>> GetAllAsync(int universeId, Page page,
            CancellationToken cancellation = default(CancellationToken));

        Task<PlayerDetailsBindingModel> GetDetailsAsync(int universeId, int playerId,
            CancellationToken cancellation = default(CancellationToken));
    }
}