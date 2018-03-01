using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Services;
using Djm.OGame.Web.Api.Services.OGame;
using Djm.OGame.Web.Api.Services.OGame.Alliances;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Alliances")]
    public class AllianceController : Controller
    {
        public IAlliancesService AlliancesService { get; }

        public AllianceController(IAlliancesService alliancesService)
        {
            AlliancesService = alliancesService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(int universeId, [ModelBinder(BinderType = typeof(PageModelBinder))]Page page)
        {
            try
            {
                var alliances = AlliancesService.GetAll(universeId, page);
                return Ok(alliances);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{allianceId:int}")]
        public IActionResult GetAlliancesDetails(int universeId,int allianceId)
        {
            try
            {
                var alliance = AlliancesService.GetDetails(universeId, allianceId);
                return Ok(alliance);
            }
            catch (OGameException e)
            {
                return BadRequest(e.Message);
            }
            
        }



    }
}