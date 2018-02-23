using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Players")]
    public class PlayersController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public IOgClient OgameClient;
        public IMapper Mapper;

        public PlayersController(IOgClient ogameClient, IMapper mapper,IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
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
        public async Task<IActionResult> GetPlayerData(int universeId, int playerId)
        {
            var players = OgameClient.Universe(universeId).GetPlayers();

            if (players == null)
                return BadRequest("Cet univers n'existe pas");

            var player = players.FirstOrDefault(p => p.Id == playerId);

            if (player == null)
                return NotFound("Ce joueur n'existe pas");

            var viewModel = Mapper.Map<PlayerDetailsBindingModel>(player);

            var pins = await UnitOfWork.Pins.ToListForOwnerAsync(playerId);

            if (!pins.Any()) return Ok(viewModel);

            var pinsWithPlayersName = OgameClient.Universe(universeId).GetPlayers()
                .Join(pins, joueur => joueur.Id, pin => pin.TargetId, (joueur, pin)
                    => new PinListItemBindingModel() { Id = pin.Id, PlayerId = joueur.Id, Name = joueur.Name });

            viewModel.Favoris = pinsWithPlayersName.ToList();

            return Ok(viewModel);
        }
    }
}