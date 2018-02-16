using System.Collections.Generic;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Planets;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Planets")]
    public class PlanetsController : Controller
    {
        public IOgClient OgameClient;
        public IMapper Mapper;

        public PlanetsController(IOgClient ogameClient, IMapper mapper)
        {
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(int universeId)
        {
            var planets = OgameClient.Universe(universeId).GetPlanets();

            if (planets == null)
                return NotFound();

            var viewModel = Mapper.Map<List<PlanetDetailsBindingModel>>(planets);

            return Ok(viewModel);
        }

    }
}