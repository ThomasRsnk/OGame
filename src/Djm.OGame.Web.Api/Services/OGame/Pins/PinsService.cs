using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Pin;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.Jobs;
using Djm.OGame.Web.Api.Services.Mails;
using Djm.OGame.Web.Api.Services.Mails.Models;
using Hangfire;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Pins
{
    public class PinsService : IPinsService
    {
        public IUnitOfWork UnitOfWork { get; }
        public IOgClient OgameClient { get; }
        public IMapper Mapper { get; }
        public IPinRepository PinRepository { get; }
        public IPlayerRepository PlayerRepository { get; }
        public IMailJob MailJob { get; }

        public PinsService(IUnitOfWork unitOfWork, IOgClient ogClient, 
            IMapper mapper,IPinRepository pinRepository,
            IPlayerRepository playerRepository,IMailJob mailJob)
        {
            UnitOfWork = unitOfWork;
            OgameClient = ogClient;
            Mapper = mapper;
            PinRepository = pinRepository;
            PlayerRepository = playerRepository;
            MailJob = mailJob;
        }

        public async Task<PinCreateBindingModel> AddPinAsync(PinCreateBindingModel bindingModel, CancellationToken cancellation = default(CancellationToken))
        {
            //vérifier que les joueurs référencés existent

            var players = OgameClient.Universe(bindingModel.UniverseId).GetPlayers();
            if (players == null)
                throw new OGameException("L'univers " + bindingModel.UniverseId + " n'existe pas");

            var owner = players.FirstOrDefault(p => p.Id == bindingModel.OwnerId);
            if (owner == null)
                throw new OGameException("owner : aucun joueur avec l'id " + bindingModel.OwnerId + " n'existe sur l'univers " + bindingModel.UniverseId);
            var target = players.FirstOrDefault(p => p.Id == bindingModel.TargetId);
            if (target == null)
                throw new OGameException("target : aucun joueur avec l'id " + bindingModel.TargetId + " n'existe sur l'univers " + bindingModel.UniverseId);

            //viewmodel => model
            var pin = Mapper.Map<Pin>(bindingModel);

            //insertion

            //PinRepository.Insert(pin);

            //SaveChanges
            //            try
            //            {
            //                await UnitOfWork.CommitAsync(cancellation);
            //            }
            //            catch (DbUpdateException e)
            //            {
            //                if (e.InnerException != null) throw new OGameException(e.InnerException.Message);
            //            }

            //model => viewmodel

            var viewModel = Mapper.Map<PinCreateBindingModel>(pin);

            //Notification mail éventuelle

            var player = await PlayerRepository.FirstOrDefaultAsync(bindingModel.UniverseId, bindingModel.TargetId, cancellation);

            if (player == null )
                return viewModel;
            if (player.EmailAddress.Equals(""))
                return viewModel;

            var pins = await PinRepository.ToListFortargetAsync(bindingModel.TargetId, bindingModel.UniverseId, cancellation);
           
            var model = new NotificationModel()
            {
                Count = pins.Count,
                FromName = owner.Name,
                ToName = target.Name
            };
            
            BackgroundJob.Enqueue(() => MailJob.SendNotificationAsync(player.EmailAddress,model));

            return viewModel;
        }

        public async Task DeletePinAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            await PinRepository.DeleteAsync(id, ct);
            await UnitOfWork.CommitAsync(ct);
        }

        public async Task<PinCreateBindingModel> GetPinAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            var pin = await PinRepository.FindAsync(id, ct);

            return pin == null ? null : Mapper.Map<PinCreateBindingModel>(pin);
        }
    }
}