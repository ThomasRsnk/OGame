using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Exceptions;
using Newtonsoft.Json;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class HttpResource
    {
        protected IHttpClient HttpClient { get; }
        public string BaseUrl { get; }

        public HttpResource(IHttpClient httpClient, string baseUrl)
        {
            HttpClient = httpClient;
            BaseUrl = baseUrl;
        }

        [DebuggerStepThrough]
        protected Task<T> JsonToPocoAsync<T>(CancellationToken cancellationToken)
            => JsonToPocoAsync<T>("", cancellationToken);

        protected async Task<T> JsonToPocoAsync<T>(string relativeUrl, CancellationToken cancellationToken)
        {
            var response = await HttpClient.GetAsync(BaseUrl + relativeUrl, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new OgameNotFoundException(BaseUrl+ relativeUrl);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

       
    }
}