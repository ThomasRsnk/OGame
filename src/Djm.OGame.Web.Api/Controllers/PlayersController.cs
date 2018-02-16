using System.Collections.Generic;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Players;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Players")]
    public class PlayersController : Controller
    {
        public IOgClient OgameClient;
        public IMapper Mapper;

        public PlayersController(IOgClient ogameClient, IMapper mapper)
        {
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(int universeId)
        {
            var players = OgameClient.Universe(universeId).GetPlayers();

            if (players == null)
                return NotFound();

            var viewModel = Mapper.Map<List<PlayerListItemBindingModel>>(players);

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("{playerId:int}")]
        public IActionResult GetPlayerData(int universeId, int playerId)
        {
            var player = OgameClient.Universe(universeId).GetPlayer(playerId);

            if (player == null)
                return NotFound("Ce joueur n'existe pas");

            var viewModel = Mapper.Map<PlayerDetailsBindingModel>(player);

            return Ok(viewModel);
        }
    }
}