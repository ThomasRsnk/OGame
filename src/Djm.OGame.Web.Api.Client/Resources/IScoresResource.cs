using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Scores;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IScoresResource
    {
        Task<List<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(int type = 0, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}