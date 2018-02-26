using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Universes;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class HttpUniversesResource : IUniversesResource
    {
        public IUniverseResource this[int universeId] => new HttpUniverseResource(universeId);

        public async Task<List<UniverseListItemViewModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var httpClient = new HttpClientAdapter(new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:53388/api/universes/1/")
            });

            return await new ServersHttpResource(httpClient).GetAllAsync(cancellationToken);
        }
    }
}