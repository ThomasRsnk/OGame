using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pins;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IPinsResource
    {
        Task<PinCreateBindingModel> Add(int ownerId,int targetId,CancellationToken ct);
        Task Delete(int pinId,CancellationToken ct);
    }
}