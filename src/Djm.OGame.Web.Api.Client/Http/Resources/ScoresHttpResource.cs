using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class ScoresHttpResource : HttpResource, IScoresResource
    {
        public ScoresHttpResource(IHttpClient httpClient) : base(httpClient, "scores/")
        {
        }

        public Task<List<ScoreListItemAllianceBindingModel>> GetAllAsync(CancellationToken cancellationToken)
            => JsonToPocoAsync<List<ScoreListItemAllianceBindingModel>>(cancellationToken);

        public Task<List<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(int type,
            CancellationToken cancellationToken)
            => JsonToPocoAsync<List<ScoreListItemPlayerBindingModel>>("players?type=" + type.ToString(CultureInfo.InvariantCulture), cancellationToken);

        public Task<List<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(CancellationToken cancellationToken)
            => JsonToPocoAsync<List<ScoreListItemAllianceBindingModel>>("alliances/", cancellationToken);
    }
}