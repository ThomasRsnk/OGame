using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class AlliancesHttpResource : HttpResource, IAlliancesResource
    {
        public AlliancesHttpResource(IHttpClient httpClient) : base(httpClient, "alliances/")
        {
        }

        public Task<List<AllianceListItemBindingModel>> GetAllAsync(CancellationToken cancellationToken)
            => JsonToPocoAsync<List<AllianceListItemBindingModel>>(cancellationToken);

        public Task<AllianceDetailsBindingModel> GetDetailsAsync(int allianceId, CancellationToken cancellationToken)
        {
            //throw  new OgameNotFoundException("alliance not found : "+ allianceId);
            return JsonToPocoAsync<AllianceDetailsBindingModel>(allianceId.ToString(CultureInfo.InvariantCulture),
                cancellationToken);

        }
    }
}