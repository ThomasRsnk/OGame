using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Services.OGame;
using Djm.OGame.Web.Api.Services.OGame.Players;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Players")]
    public class PlayersController : Controller
    {
        public IPlayersService PlayersService { get; }
        
        public PlayersController(IPlayersService playersService)
        {
            PlayersService = playersService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAll(int universeId,Page page, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var players = await PlayersService.GetAllAsync(universeId, page, cancellationToken);
                return Ok(players);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Route("{playerId:int}")]
        public async Task<IActionResult> GetPlayerData(int universeId, int playerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var player = await PlayersService.GetDetailsAsync(universeId, playerId, cancellationToken);
                return Ok(player);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}