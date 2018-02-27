using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class AlliancesHttpResource : HttpResource, IAlliancesResource
    {
        public AlliancesHttpResource(IHttpClient httpClient) : base(httpClient, "alliances/")
        {
        }

        public Task<List<AllianceListItemBindingModel>> GetAllAsync(int skip,int take,CancellationToken cancellationToken)
            => JsonToPocoAsync<List<AllianceListItemBindingModel>>("?skip=" + skip + "&take=" + take,cancellationToken);

        public Task<AllianceDetailsBindingModel> GetDetailsAsync(int allianceId, CancellationToken cancellationToken)
            => JsonToPocoAsync<AllianceDetailsBindingModel>(allianceId.ToString(CultureInfo.InvariantCulture),
                cancellationToken);
    }
}