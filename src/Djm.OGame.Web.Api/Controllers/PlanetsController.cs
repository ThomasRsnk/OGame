using System.Collections.Generic;
using System.Linq;
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
        public IActionResult GetAll(int universeId,int skip=0,int take = 30_000)
        {
            var planets = OgameClient.Universe(universeId).GetPlanets();

            if (planets == null) return NotFound();

            planets = planets.Skip(skip).Take(take).ToList();

            var viewModel = Mapper.Map<List<PlanetDetailsBindingModel>>(planets);

            return Ok(viewModel);
        }

    }
}