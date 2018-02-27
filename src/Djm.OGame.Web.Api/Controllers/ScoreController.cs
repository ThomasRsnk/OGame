using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Scores")]
    public class ScoreController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public IOgClient OgameClient;
        public IMapper Mapper;

        public ScoreController(IOgClient ogameClient, IMapper mapper,IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("players")]
        public async Task<IActionResult> GetAllForPlayers(int universeId,int type=0,int skip=0,int take=3000)
        {
            var scores = OgameClient.Universe(universeId).GetPlayersScores(type);

            if (scores == null ) return NotFound();

            scores = scores.Skip(skip).Take(take).ToList();

            var viewModel = Mapper.Map<List<ScoreListItemPlayerBindingModel>>(scores);

            foreach (var vm in viewModel)
            {
                var tuple = await UnitOfWork.Players.FirstOrDefaultAsync(universeId, vm.Player.Id);
                if (tuple != null)
                    vm.Player.ProfilePicUrl = "http://localhost:53388/api/universes/" + universeId + "/players/" + vm.Player.Id +
                                       "/profilepic";
            }

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("alliances")]
        public IActionResult GetAllForAlliances(int universeId,int skip=0,int take=2000)
        {
            var scores = OgameClient.Universe(universeId).GetAllianceScores();

            if (scores == null) return NotFound("L'univers n'existe pas");

            scores = scores.Skip(skip).Take(take).ToList();

            var viewModel = Mapper.Map<List<ScoreListItemAllianceBindingModel>>(scores);

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("players/id/{playerId:int}")]//positions d'un joueur au sein des différents classements
        public IActionResult GetPlayerScores(int universeId, int playerId)
        {
            var positions = OgameClient.Universe(universeId).GetPositions(playerId);

            if (positions == null) return NotFound();

            var viewModel = Mapper.Map<List<PositionsBindingModel>>(positions);

            return Ok(viewModel);
        }

        


    }
}