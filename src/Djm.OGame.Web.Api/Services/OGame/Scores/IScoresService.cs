using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Scores;

namespace Djm.OGame.Web.Api.Services.OGame.Scores
{
    public interface IScoresService
    {
        Task<PagedListViewModel<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(int type,int universeId, Page page,
            CancellationToken cancellation = default(CancellationToken));

        PagedListViewModel<ScoreListItemAllianceBindingModel> GetAllForAlliances(int universeId, Page page);
    }
}