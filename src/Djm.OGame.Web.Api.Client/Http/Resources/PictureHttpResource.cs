using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client.Exceptions;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http.Resources
{
    public class PicturesHttpResource : HttpResource, IPictureResource
    {
        public PicturesHttpResource(IHttpClient httpClient) : base(httpClient, "players/")
        { }

        public async Task<bool> Set(int playerId, string path, CancellationToken ct)
        {
            //lecture du fichier et transformation en byte[]
            var image = File.OpenRead(path);
            
            if (image == null)
                throw new OgameException("Error reading image file");

            var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            //construction de la requête
            var contentType = "image/" + Path.GetExtension(image.Name).Substring(1);
            var url = BaseUrl + playerId + "/profilepic";
            var requestContent = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(ms.ToArray());
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse(contentType);

            requestContent.Add(imageContent, "pic","image.jpg");

            //envoi
            var response = HttpClient.PostAsync(url, requestContent,ct).Result;

            if (response.IsSuccessStatusCode) return true;

            //traitement erreurs
    
            var body = response.Content.ReadAsStringAsync().Result;
            throw new OgameException(body);
            
        }

        public string Get(int playerId)
        {
            return BaseUrl + playerId + "/profilepic";
        }
    }
}