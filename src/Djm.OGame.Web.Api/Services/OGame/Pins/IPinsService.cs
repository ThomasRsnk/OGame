using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pins;

namespace Djm.OGame.Web.Api.Services.OGame.Pins
{
    public interface IPinsService
    {
        Task<PinCreateBindingModel> AddPinAsync(PinCreateBindingModel bindingModel,
            CancellationToken cancellation = default(CancellationToken));

        Task DeletePinAsync(int id, CancellationToken ct = default(CancellationToken));

        Task<PinCreateBindingModel> GetPinAsync(int id, CancellationToken ct = default(CancellationToken));
    }
}