using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Account;
using Djm.OGame.Web.Api.Dal.Entities;

namespace Djm.OGame.Web.Api.Services.Authentication
{
    public interface IAccountService
    {
        Task<Player> CheckPasswordAsync(LoginBindingModel credentials, CancellationToken cancellation=default(CancellationToken));

        Task RegisterUser(RegisterBindingModel bindingModel,
            CancellationToken cancellation = default(CancellationToken));
    }
}