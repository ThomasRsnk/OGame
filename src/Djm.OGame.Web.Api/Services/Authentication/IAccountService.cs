using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.ViewModels.Account;
using LoginViewModelModel = Djm.OGame.Web.Api.BindingModels.Account.LoginViewModelModel;

namespace Djm.OGame.Web.Api.Services.Authentication
{
    public interface IAccountService
    {
        Task<Player> CheckPasswordAsync(LoginViewModelModel credentials, CancellationToken cancellation=default(CancellationToken));

        Task RegisterUser(RegisterViewModel bindingModel,
            CancellationToken cancellation = default(CancellationToken));
    }
}