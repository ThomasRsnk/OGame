using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.Helpers;
using Djm.OGame.Web.Api.Services.OGame;
using Djm.OGame.Web.Api.ViewModels.Account;
using Microsoft.EntityFrameworkCore;
using OGame.Client;
using LoginViewModelModel = Djm.OGame.Web.Api.BindingModels.Account.LoginViewModelModel;


namespace Djm.OGame.Web.Api.Services.Authentication
{
    public class AccountService : IAccountService
    {
        public IPlayerRepository PlayerRepository { get; }
        public IOgClient OgClient { get; }
        public IUnitOfWork UnitOfWork { get; }

        public AccountService(IPlayerRepository playerRepository,IOgClient ogClient,IUnitOfWork unitOfWork)
        {
            PlayerRepository = playerRepository;
            OgClient = ogClient;
            UnitOfWork = unitOfWork;
        }

        public async Task<Player> CheckPasswordAsync(LoginViewModelModel credentials,CancellationToken cancellation)
        {
            var player = await PlayerRepository.FirstOrDefaultAsync(credentials.Email, cancellation);

            if (player == null) return null;

            return !player.Password.Equals(credentials.Password.ToHash(player.Salt,out var x)) ? null : player;
        }

        public async Task RegisterUser(RegisterViewModel bindingModel, CancellationToken cancellation = default(CancellationToken))
        {
            var players = OgClient.Universe(bindingModel.UniverseId).GetPlayers();
            if (players == null)
                throw new OGameException("L'univers " + bindingModel.UniverseId + " n'existe pas");

            var player = players.FirstOrDefault(p => p.Id == bindingModel.PlayerId);
            if (player == null)
                throw new OGameException("Aucun joueur avec l'id " + bindingModel.PlayerId + " n'existe sur l'univers " + bindingModel.UniverseId);

            var playerInDb = await PlayerRepository.FirstOrDefaultAsync(bindingModel.Email, cancellation);

            if(playerInDb != null)
                throw new OGameException("Cette adresse email est déjà utilisée");

            var newPlayer = new Player
            {
                EmailAddress = bindingModel.Email,
                Password  = bindingModel.Password.ToHash(null,out var salt),
                Salt = salt,
                UniverseId = bindingModel.UniverseId,
                OGameId = bindingModel.PlayerId,
                Name = player.Name,
                Role = Roles.Utilisateur
            };

            PlayerRepository.Insert(newPlayer);

            try
            {
                await UnitOfWork.CommitAsync(cancellation);
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null) throw new OGameException(e.InnerException.Message);
            }
            

        }
    }

    
}