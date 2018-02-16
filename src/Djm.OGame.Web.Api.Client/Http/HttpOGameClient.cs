using Djm.OGame.Web.Api.Client.Http.Resources;
using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client.Http
{
    public class HttpOGameClient : IOGameClient
    {
        public IUniversesResource Universes => new HttpUniversesResource();
    }
}