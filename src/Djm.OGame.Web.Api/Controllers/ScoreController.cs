using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Services.OGame;
using Djm.OGame.Web.Api.Services.OGame.Scores;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Scores")]
    public class ScoreController : Controller
    {
        public IScoresService ScoresService { get; }


        public ScoreController(IScoresService scoresService)
        {
            ScoresService = scoresService;
        }

        [HttpGet]
        [Route("players")]
        public async Task<IActionResult> GetAllForPlayers(int universeId, Page page,int type=0)
        {
            try
            {
                var scores = await ScoresService.GetAllForPlayersAsync(type, universeId, page);
                return Ok(scores);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("alliances")]
        public IActionResult GetAllForAlliances(int universeId, Page page)
        {
            try
            {
                var scores = ScoresService.GetAllForAlliances(universeId, page);
                return Ok(scores);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}