using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Scores
{
    public class ScoreService : IScoresService
    {
        public IOgClient OgameClient { get; }
        public IMapper Mapper { get; }
        public IPlayerRepository PlayerRepository { get; }


        public ScoreService(IOgClient ogClient, IMapper mapper,IPlayerRepository playerRepository)
        {
            OgameClient = ogClient;
            Mapper = mapper;
            PlayerRepository = playerRepository;
        }

        public async Task<PagedListViewModel<ScoreListItemPlayerBindingModel>> GetAllForPlayersAsync(int type, int universeId, Page page, CancellationToken cancellation)
        {
            var scores = OgameClient.Universe(universeId).GetPlayersScores(type);

            if (scores == null) throw new OGameException("L'univers n'existe pas");

            var count = scores.Count;

            scores = scores.Paginate(page).ToList();

            var playersFromDb = await PlayerRepository.ToListAsync(universeId, cancellation);

            var viewModels = scores.GroupJoin(
                playersFromDb, p1 => p1.Id, p2 => p2.Id, (p1, p2) => new ScoreListItemPlayerBindingModel()
                {
                    Player = new PlayerListItemBindingModel()
                    {
                        Id = p1.Player.Id,
                        Name = p1.Player.Name,
                        ProfilePicUrl = p2.SingleOrDefault() == null ? null : "~/api/universes/" + universeId + "/players/" + p1.Id +
                                                                                "/profilepic"
                    },
                    Points = p1.Points,
                    Rank = p1.Rank
                   
                }).ToList();

            return PagedListViewModel.Create(viewModels, count, page);
        }

        public PagedListViewModel<ScoreListItemAllianceBindingModel> GetAllForAlliances(int universeId, Page page)
        {
            var scores = OgameClient.Universe(universeId).GetAllianceScores();

            if (scores == null) throw new OGameException("L'univers n'existe pas");

            var viewModel = Mapper.Map<List<ScoreListItemAllianceBindingModel>>(scores);

            return viewModel.ToPagedListViewModel(page);
        }

    }
}