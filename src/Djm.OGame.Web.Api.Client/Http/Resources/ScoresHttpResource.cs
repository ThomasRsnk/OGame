using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class ScoresHttpResource : HttpResource, IScoresResource
    {
        public ScoresHttpResource(IHttpClient httpClient) : base(httpClient, "scores/")
        {
        }

        public Task<PagedListViewModel<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(Page page,Classement type,
            CancellationToken cancellationToken)
        {

            page = page ?? Page.Default;
            return JsonToPocoAsync<PagedListViewModel<ScoreListItemPlayerBindingModel>>(
                "players?type=" + (int) type + "&page=" + page.Current + "&pageLength=" + page.Size, cancellationToken);
        }

        public Task<PagedListViewModel<ScoreListItemAllianceBindingModel>> GetAllForAlliancesAsync(Page page, CancellationToken cancellationToken)
        {
            page = page ?? Page.Default;
            return JsonToPocoAsync<PagedListViewModel<ScoreListItemAllianceBindingModel>>(
                "alliances" + "?page=" + page.Current + "&pageLength=" + page.Size, cancellationToken);
        }
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