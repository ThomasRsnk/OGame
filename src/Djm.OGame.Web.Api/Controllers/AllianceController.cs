using System.Collections.Generic;
using System.Linq;
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
        public IActionResult GetAll(int universeId,int skip=0,int take=2000)
        {
            var alliances = OgameClient.Universe(universeId).GetAlliances();

            if (alliances == null) return NotFound("Cet univers n'existe pas");

            alliances = alliances.Skip(skip).Take(take).ToList();

            var viewModel = Mapper.Map<List<AllianceListItemBindingModel>>(alliances);

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("{allianceId:int}")]
        public IActionResult GetAlliancesDetails(int universeId,int allianceId)
        {
            var alliances = OgameClient.Universe(universeId).GetAlliances();

            if (alliances == null) return NotFound("Cet univers n'existe pas");

            var alliance = alliances.FirstOrDefault(a => a.Id == allianceId);

            if (alliance == null) return NotFound("Cette alliance n'existe pas");

            var viewModel = Mapper.Map<AllianceDetailsBindingModel>(alliance);

            return Ok(viewModel);
        }



    }
}