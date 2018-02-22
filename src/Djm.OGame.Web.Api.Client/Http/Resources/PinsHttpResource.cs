using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Resources;
using Newtonsoft.Json;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class PinsHttpResource : HttpResource,IPinsResource
    {
        public PinsHttpResource(IHttpClient httpClient) : base(httpClient, "pins/")
        {
        }

        public async Task<PinCreateBindingModel> Add(int ownerId, int targetId, CancellationToken ct)
        {
            var pin = new PinCreateBindingModel() {OwnerId = ownerId, TargetId = targetId};

            var jsonObject = JsonConvert.SerializeObject(pin);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            
            var response = HttpClient.PostAsync(BaseUrl, content, ct).Result;
          
            var body = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode != HttpStatusCode.BadRequest)
                return JsonConvert.DeserializeObject<PinCreateBindingModel>(body);

            body.Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":"," : ");
            throw new OgameException(body);

        }

        public async Task Delete(int pinId, CancellationToken ct)
        {
            await HttpClient.DeleteAsync(BaseUrl+pinId, ct);
        }

        
    }
}