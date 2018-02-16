using Djm.OGame.Web.Api.Client.Resources;

namespace Djm.OGame.Web.Api.Client
{
    public interface IOGameClient
    {
        IUniversesResource Universes { get; }
    }
}