using System;
using System.Data.Entity.Validation;
using System.Linq;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Dal.Models;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;


namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/universes/{universeId:int}/pins")]
    public class PinsController : Controller
    {
        public IMapper Mapper;
        public IOgameDb OGameDbProvider;
        public IOgClient OgameClient;

        public PinsController(IOgameDb oGameDbProvider, IMapper mapper,IOgClient ogclient)
        {
            OGameDbProvider = oGameDbProvider;
            Mapper = mapper;
            OgameClient = ogclient;
        }

        [HttpPost]
        public IActionResult AddPin([FromBody] PinCreateBindingModel bindingModel,int universeId)
        {
            //check body empty
            if (bindingModel == null)
                return BadRequest();
            //ajouter l'univers (obtenu par l'url)
            bindingModel.UniverseId = universeId;

            //vérifier que les champs sont valides
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //vérifier que les joueurs référencés existent

            var players = OgameClient.Universe(universeId).GetPlayers();
            if (players.FirstOrDefault(p => p.Id == bindingModel.OwnerId) == null)
                return BadRequest("owner : aucun joueur avec l'id " + bindingModel.OwnerId + " n'existe sur l'univers " + bindingModel.UniverseId);
            if (players.FirstOrDefault(p => p.Id == bindingModel.TargetId) == null)
                return BadRequest("target : aucun joueur avec l'id " + bindingModel.TargetId + " n'existe sur l'univers "+bindingModel.UniverseId);

            //viewmodel => model
            var pin = Mapper.Map<Pin>(bindingModel);

            //insertion
            try
            {
                pin = OGameDbProvider.Pins.Insert(pin);
            }
            catch (DbEntityValidationException ex)
            {
                //uniqueness violated
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return BadRequest(ModelState);
            }
            
            //model => viewmodel
            
            var viewModel = Mapper.Map<PinCreateBindingModel>(pin);

            return Created(Url.Action("GetPin", new { id = pin.Id }), viewModel);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetPin(int id)
        {
            var pin = OGameDbProvider.Pins.FirstOrDefault(id);

            if (pin == null) return NotFound();

            return Ok(pin);
        }

        [HttpGet]
        [Route("player/{playerId:int}")]
        public IActionResult GetPinsForPlayer(int playerId,int universeId)
        {
            var pins = OGameDbProvider.Pins.ToList(playerId);

            if (!pins.Any()) return NotFound();

            var players = OgameClient.Universe(universeId).GetPlayers()
                .Join(pins, player => player.Id, pin => pin.TargetId, (player, pin)
                    => new PinListItemBindingModel() {Id = pin.Id, PlayerId = player.Id, Name = player.Name}); 

            return Ok(players);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeletePin(int id)
        {
            OGameDbProvider.Pins.Delete(id);

            return NoContent();
        }
    }
}