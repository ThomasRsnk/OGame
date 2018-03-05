using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Client.Http.Resources;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IScoresResource
    {
        Task<PagedListViewModel<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(Page page,Classement type = 0,CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedListViewModel<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(Page page, CancellationToken cancellationToken = default(CancellationToken));
    }
}