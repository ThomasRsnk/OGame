using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.Dal.Repositories.Pin;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Players
{
    public class PlayerService : IPlayersService
    {
        public PlayerService(IPinRepository pinRepository,IPlayerRepository playerRepository, IOgClient ogClient, IMapper mapper)
        {
            PinRepository = pinRepository;
            PlayerRepository = playerRepository;
            OgameClient = ogClient;
            Mapper = mapper;
        }

        public IPinRepository PinRepository { get; }
        public IPlayerRepository PlayerRepository { get; }
        public IOgClient OgameClient { get; }
        public IMapper Mapper { get; }


        public async Task<PagedListViewModel<PlayerListItemBindingModel>> GetAllAsync(int universeId, Page page, CancellationToken cancellation = default(CancellationToken))
        {
            var players = OgameClient.Universe(universeId).GetPlayers();

            if (players == null) throw new OGameException("L'univers n'existe pas");
            
            var count = players.Count;

            players = players.Paginate(page).ToList();
           
            var playersFromDb = await PlayerRepository.ToListAsync(universeId, cancellation);

            var viewModels = players.GroupJoin(
                playersFromDb, p1 => p1.Id, p2 => p2.Id, (p1, p2) => new PlayerListItemBindingModel()
                {
                    Id = p1.Id,
                    Name = p1.Name,
                    ProfilePicUrl = p2.SingleOrDefault() == null ? null : "~/api/universes/" + universeId + "/players/" + p1.Id +
                                                                          "/profilepic"
                }).ToList();
            
            return PagedListViewModel.Create(viewModels, count, page);
        }

        public async Task<PlayerDetailsBindingModel> GetDetailsAsync(int universeId, int playerId, CancellationToken cancellation = default(CancellationToken))
        {
            var players = OgameClient.Universe(universeId).GetPlayers();

            if (players == null)
                throw new OGameException("Cet univers n'existe pas");

            var player = players.FirstOrDefault(p => p.Id == playerId);

            if (player == null)
                throw new OGameException("Ce joueur n'existe pas");

            var viewModel = Mapper.Map<PlayerDetailsBindingModel>(player);

            //PICTURE

            var tuple = await PlayerRepository.FirstOrDefaultAsync(universeId, playerId, cancellation);

            if (tuple != null)
                viewModel.ProfilePicUrl = "~/api/universes/"
                                          + universeId + "/players/" + tuple.Id
                                          + "/profilepic";

            //FAVORIS

            var pins = await PinRepository.ToListForOwnerAsync(playerId,universeId, cancellation);

            if (!pins.Any()) return viewModel;

            var pinsWithPlayersName = OgameClient.Universe(universeId).GetPlayers()
                .Join(pins, joueur => joueur.Id, pin => pin.TargetId, (joueur, pin)
                    => new PinListItemBindingModel() { Id = pin.Id, PlayerId = joueur.Id, Name = joueur.Name });

            viewModel.Favoris = pinsWithPlayersName.ToList();

            return viewModel;
        }
    }
}