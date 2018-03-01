using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Universes;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class ServersHttpResource : HttpResource, IServersResource
    {
        public ServersHttpResource(IHttpClient httpClient) : base(httpClient, "")
        {
        }

        public Task<List<UniverseListItemViewModel>> GetAllAsync(CancellationToken cancellationToken)
            => JsonToPocoAsync<List<UniverseListItemViewModel>>(cancellationToken);
    }
}