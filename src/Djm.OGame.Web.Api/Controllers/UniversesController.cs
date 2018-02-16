using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes")]
    public class UniversesController : Controller
    {
        public IOgClient OgameClient;
        public IMapper Mapper;

        public UniversesController(IOgClient ogameClient, IMapper mapper)
        {
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("{universeId:int?}")]
        public IActionResult GetUniverses(int universeId=1)
        {
            var universes = OgameClient.Universe(universeId).GetUniverses();

            if (universes == null)
                return NotFound();

            return Ok(universes);
        }

        [HttpGet]
        [Route("{universeId:int}/connection/{pseudo}")]
        public IActionResult Connect(int universeId, string pseudo)
        {
            if (OgameClient.Universe(universeId).GetPlayers().Exists(p =>p.Name == pseudo))
                return Ok();

            return NotFound();
        }
    }
}