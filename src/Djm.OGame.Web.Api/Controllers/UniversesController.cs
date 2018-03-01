using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services.OGame.Universes;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes")]
    public class UniversesController : Controller
    {
        public IUniversService UniversService { get; }


        public UniversesController(IUniversService universService)
        {
            UniversService = universService;
        }

        [HttpGet]
        [Route("{universeId:int?}")]
        public async Task<IActionResult> GetUniverses(int universeId=1)
        {
            var universes = await UniversService.GetUniverses();

            if (universes == null) return NotFound();

            return Ok(universes);
        }

       
    }
}