using System.Collections.Generic;
using System.ComponentModel;
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

        public Task<List<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(Classement type,int skip,int take,
            CancellationToken cancellationToken)
            => JsonToPocoAsync<List<ScoreListItemPlayerBindingModel>>("players?type=" + (int)type+ "&skip=" + skip + "&take=" + take, cancellationToken);

        public Task<List<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(int skip,int take,CancellationToken cancellationToken)
            => JsonToPocoAsync<List<ScoreListItemAllianceBindingModel>>("alliances"+ "?skip=" + skip + "&take=" + take, cancellationToken);
    }

    
    public enum Classement 
    {
        General,
        Economie,
        Recherche,
        Militaire,
        MilitairesPerdus,
        MilitairesConstruits,
        MilitairesDetruits,
        Honorifique
    }
}