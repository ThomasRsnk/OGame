using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class AlliancesHttpResource : HttpResource, IAlliancesResource
    {
        public AlliancesHttpResource(IHttpClient httpClient) : base(httpClient, "alliances/")
        {
        }

        public Task<PagedListViewModel<AllianceListItemBindingModel>> GetAllAsync(int page , int pageLength, CancellationToken cancellationToken)
            => JsonToPocoAsync<PagedListViewModel<AllianceListItemBindingModel>>("?page=" + page + "&pageLength=" + pageLength, cancellationToken);

        public Task<AllianceDetailsBindingModel> GetDetailsAsync(int allianceId, CancellationToken cancellationToken)
            => JsonToPocoAsync<AllianceDetailsBindingModel>(allianceId.ToString(CultureInfo.InvariantCulture),
                cancellationToken);
    }
}