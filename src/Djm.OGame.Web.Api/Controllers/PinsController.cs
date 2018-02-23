using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OGame.Client;


namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/universes/{universeId:int}/pins")]
    public class PinsController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper;
        public IOgClient OgameClient;

        public PinsController(IUnitOfWork unitOfWork, IMapper mapper,IOgClient ogclient)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            OgameClient = ogclient;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPin([FromBody] PinCreateBindingModel bindingModel,int universeId)
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
            if (players == null)
                return BadRequest("L'univers " + universeId + " n'existe pas");
            if (players.FirstOrDefault(p => p.Id == bindingModel.OwnerId) == null)
                return BadRequest("owner : aucun joueur avec l'id " + bindingModel.OwnerId + " n'existe sur l'univers " + bindingModel.UniverseId);
            if (players.FirstOrDefault(p => p.Id == bindingModel.TargetId) == null)
                return BadRequest("target : aucun joueur avec l'id " + bindingModel.TargetId + " n'existe sur l'univers "+bindingModel.UniverseId);

            //viewmodel => model
            var pin = Mapper.Map<Pin>(bindingModel);

            //insertion

            UnitOfWork.Pins.InsertAsync(pin);

            //SaveChanges
            try
            {
                await UnitOfWork.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null) return BadRequest(e.InnerException.Message);
            }
            
            //model => viewmodel

            var viewModel = Mapper.Map<PinCreateBindingModel>(pin);

            return Created(Url.Action("GetPin", new { id = pin.Id }), viewModel);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetPin(int id)
        {
            var pin = await UnitOfWork.Pins.FindAsync(id);

            if (pin == null) return NotFound();

            return Ok(pin);
        }

        [HttpGet]
        [Route("player/{playerId:int}")]
        public async Task<IActionResult> GetPinsForPlayer(int playerId,int universeId)
        {
            var pins = await UnitOfWork.Pins.ToListForOwnerAsync(playerId);

            if (!pins.Any()) return NotFound();

            var players = OgameClient.Universe(universeId).GetPlayers()
                .Join(pins, player => player.Id, pin => pin.TargetId, (player, pin)
                    => new PinListItemBindingModel() {Id = pin.Id, PlayerId = player.Id, Name = player.Name}); 

            return Ok(players); 
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditPin(int id, int targetId)
        {
            var pin = await UnitOfWork.Pins.FindAsync(id);

            if (pin == null) return NotFound();

            UnitOfWork.Pins.Update(pin);
            await UnitOfWork.CommitAsync();
            return Ok(pin);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeletePin(int id)
        {
            await UnitOfWork.Pins.DeleteAsync(id);
            await UnitOfWork.CommitAsync();
            return NoContent();
        }
    }
}