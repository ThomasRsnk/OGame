using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Client.Http.Resources;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IScoresResource
    {
        Task<PagedListViewModel<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(Classement type = 0,int page=0,int pageLength=50, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedListViewModel<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(int page = 0, int pageLength = 50, CancellationToken cancellationToken = default(CancellationToken));
    }
}