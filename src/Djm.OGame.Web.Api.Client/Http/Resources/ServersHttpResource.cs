using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class ServersHttpResource : HttpResource, IServersResource
    {
        public ServersHttpResource(IHttpClient httpClient) : base(httpClient, "")
        {
        }

        public Task<Dictionary<int, string>> GetAllAsync(CancellationToken cancellationToken)
            => JsonToPocoAsync<Dictionary<int, string>>(cancellationToken);
    }
}