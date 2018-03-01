using System;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.Services;
using Djm.OGame.Web.Api.Services.OGame.Pins;
using Microsoft.AspNetCore.Mvc;


namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/universes/{universeId:int}/pins")]
    public class PinsController : Controller
    {
        public IPinsService PinsService { get; }

        public PinsController(IPinsService pinsService)
        {
            PinsService = pinsService;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPin(PinCreateBindingModel bindingModel,int universeId)
        {
            //check body empty
            if (bindingModel == null)
                return BadRequest();

            //ajouter l'univers (obtenu par l'url)
            bindingModel.UniverseId = universeId;

            //vérifier que les champs sont valides
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pin = await PinsService.AddPinAsync(bindingModel);

                return Created(Url.Action("GetPin", new { id = pin.Id }), bindingModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetPin(int id)
        {
            var viewModel = await PinsService.GetPinAsync(id);

            if (viewModel == null) return NotFound();

            return Ok(viewModel);
        }

        

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeletePin(int id)
        {
            await PinsService.DeletePinAsync(id);
            return NoContent();
        }
    }
}