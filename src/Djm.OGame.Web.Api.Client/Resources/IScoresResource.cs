using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Client.Http.Resources;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IScoresResource
    {
        Task<List<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(Classement type = 0,int skip=0,int take=int.MaxValue, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(int skip=0,int take=int.MaxValue,CancellationToken cancellationToken = default(CancellationToken));
    }
}