﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Client.Http
{
    public class HttpClientAdapter : IHttpClient
    {
        public HttpClient HttpClient { get; }


        public HttpClientAdapter(HttpClient httpClient)
        {
            HttpClient = httpClient;
            Url = httpClient.BaseAddress.ToString();
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken) 
            => HttpClient.GetAsync(requestUri, cancellationToken);

        public Task<HttpResponseMessage> PostAsync(string requestUri,HttpContent content, CancellationToken cancellationToken)
            =>  HttpClient.PostAsync(requestUri, content, cancellationToken);

        public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
            => HttpClient.DeleteAsync(requestUri, cancellationToken);

        public string Url { get; }
    }
}