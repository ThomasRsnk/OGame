using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Controllers;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Player;


namespace Djm.OGame.Web.Api.Services.Authentication
{
    public class AccountService : IAccountService
    {
        public IPlayerRepository PlayerRepository { get; }

        public AccountService(IPlayerRepository playerRepository)
        {
            PlayerRepository = playerRepository;
        }

        public async Task<Player> CheckPasswordAsync(LoginBindingModel credentials,CancellationToken cancellation)
        {
            var player = await PlayerRepository.FirstOrDefaultAsync(credentials.UserName, cancellation);

            if (player == null) return null;

            if (!player.Password.Equals(credentials.Password)) return null;

            return player;
        }
        
    }

    
}