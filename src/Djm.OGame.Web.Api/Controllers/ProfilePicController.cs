using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services.OGame.Pictures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/api/users/{email}/[Controller]")]
    public class ProfilePicController : Controller
    {
        public ProfilePicController(IPictureService pictureService)
        {
            PictureService = pictureService;
        }

        internal IPictureService PictureService { get; }


        [HttpPost]
        public async Task<IActionResult> AddProfilePic(string email, IFormFile pic,CancellationToken cancellation = default(CancellationToken))
        {
            try
            {
                await PictureService.SavePictureAsync(email, pic, cancellation);
            }
            catch (PictureException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilePic(string email, CancellationToken cancellation = default(CancellationToken))
        {
            var image = await PictureService.GetAsync(email, cancellation);

            if (image == null)
                return NotFound();

            var contentType = "image/" + Path.GetExtension(image.Name)?.Substring(1);

            return File(image, contentType);
        }



        
    }
}