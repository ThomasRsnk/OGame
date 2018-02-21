using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Client.Http
{
    public interface IHttpClient
    {
        /// <summary>Send a GET request to the specified Uri with a cancellation token as an asynchronous operation.</summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestUri" /> was <see langword="null" />.</exception>
        /// <exception cref="T:System.Net.Http.HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostAsync(string requestUri, StringContent obj, CancellationToken cancellationToken);

        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);
    }
}