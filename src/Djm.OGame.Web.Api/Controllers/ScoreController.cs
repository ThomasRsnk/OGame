using System;
using System.Collections.Generic;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Scores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Scores")]
    public class ScoreController : Controller
    {
        public IOgClient OgameClient;
        public IMapper Mapper;

        public ScoreController(IOgClient ogameClient, IMapper mapper)
        {
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("players/{type:int?}")]//classement des joueurs
        public IActionResult GetAllForPlayers(int universeId,int type=0)
        {
            var scores = OgameClient.Universe(universeId).GetPlayersScores(type);

            if (scores == null)
                return NotFound();

            var viewModel = Mapper.Map<List<ScoreListItemPlayerBindingModel>>(scores);

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("alliances")]//classement des joueurs
        public IActionResult GetAllForAlliances(int universeId)
        {
            var scores = OgameClient.Universe(universeId).GetAllianceScores();

            if (scores == null)
                return NotFound();

            var viewModel = Mapper.Map<List<ScoreListItemAllianceBindingModel>>(scores);

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("players/id/{playerId:int}")]//positions d'un joueur au sein des diff classements
        public IActionResult GetPlayerScores(int universeId, int playerId)
        {
            var positions = OgameClient.Universe(universeId).GetPositions(playerId);

            if (positions == null) return NotFound();

            var viewModel = Mapper.Map<List<PositionsBindingModel>>(positions);

            return Ok(viewModel);
        }

        


    }
}