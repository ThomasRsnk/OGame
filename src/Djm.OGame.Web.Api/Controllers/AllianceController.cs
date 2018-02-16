using System.Collections.Generic;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes/{universeId:int}/Alliances")]
    public class AllianceController : Controller
    {
        public IOgClient OgameClient;
        public IMapper Mapper;

        public AllianceController(IOgClient ogameClient, IMapper mapper)
        {
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll(int universeId)
        {
            var alliances = OgameClient.Universe(universeId).GetAlliances();

            if (alliances == null)
                return NotFound();

            var viewModel = Mapper.Map<List<AllianceListItemBindingModel>>(alliances);

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("{allianceId:int}")]
        public IActionResult GetAlliancesDetails(int universeId,int allianceId)
        {
            var alliance = OgameClient.Universe(universeId).GetAlliance(allianceId);

            if (alliance == null)
                return NotFound();

            var viewModel = Mapper.Map<AllianceDetailsBindingModel>(alliance);

            return Ok(viewModel);
        }



    }
}