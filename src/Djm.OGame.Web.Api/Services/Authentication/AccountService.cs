using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Account;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.Helpers;
using Djm.OGame.Web.Api.Services.OGame;
using Microsoft.EntityFrameworkCore;
using OGame.Client;


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

        public async Task<Player> CheckPasswordAsync(LoginBindingModel credentials,CancellationToken cancellation)
        {
            var player = await PlayerRepository.FirstOrDefaultAsync(credentials.UserName, cancellation);

            if (player == null) return null;

            return !player.Password.Equals(credentials.Password.ToHash(player.Salt,out var x)) ? null : player;
        }

        public async Task RegisterUser(RegisterBindingModel bindingModel, CancellationToken cancellation = default(CancellationToken))
        {
            var players = OgClient.Universe(bindingModel.UniverseId).GetPlayers();
            if (players == null)
                throw new OGameException("L'univers " + bindingModel.UniverseId + " n'existe pas");

            var player = players.FirstOrDefault(p => p.Id == bindingModel.PlayerId);
            if (player == null)
                throw new OGameException("Aucun joueur avec l'id " + bindingModel.PlayerId + " n'existe sur l'univers " + bindingModel.UniverseId);
            
            var newPlayer = new Player
            {
                Login = bindingModel.UserName,
                Password  = bindingModel.Password.ToHash(null,out var salt),
                Salt = salt,
                UniverseId = bindingModel.UniverseId,
                Id = bindingModel.PlayerId,
                Name = player.Name
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