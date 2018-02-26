using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/Api/Universes")]
    public class UniversesController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public IOgClient OgameClient;
        public IMapper Mapper;

        public UniversesController(IOgClient ogameClient, IMapper mapper,IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            OgameClient = ogameClient;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("{universeId:int?}")]
        public async Task<IActionResult> GetUniverses(int universeId=1)
        {
            var uni = await UnitOfWork.Univers.ToListAsync();
            
            if (uni.Any()) return Ok(uni);

            var universes = OgameClient.Universe(universeId).GetUniverses();

            if (universes == null)
                return NotFound();

            foreach(var entry in universes)
                UnitOfWork.Univers.Insert(new Univers(){Id = entry.Id,Name = entry.Name});

            await UnitOfWork.CommitAsync();
            uni = await UnitOfWork.Univers.ToListAsync();

            return Ok(uni);
        }

       
    }
}