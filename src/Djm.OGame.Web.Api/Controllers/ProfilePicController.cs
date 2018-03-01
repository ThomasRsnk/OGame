using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services.OGame.Pictures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/api/universes/{universeId:int}/players/{playerId:int}/[Controller]")]
    public class ProfilePicController : Controller
    {
        public ProfilePicController(IPictureService pictureService)
        {
            PictureService = pictureService;
        }

        internal IPictureService PictureService { get; }


        [HttpPost]
        public async Task<IActionResult> AddProfilePic(int universeId,int playerId, IFormFile pic,CancellationToken cancellation = default(CancellationToken))
        {
            try
            {
                await PictureService.SavePictureAsync(universeId, playerId, pic, cancellation);
            }
            catch (PictureException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilePic(int universeId, int playerid, CancellationToken cancellation = default(CancellationToken))
        {
            var image = await PictureService.GetAsync(universeId, playerid, cancellation);

            if (image == null)
                return NotFound();

            var contentType = "image/" + Path.GetExtension(image.Name)?.Substring(1);

            return File(image, contentType);
        }



        
    }
}