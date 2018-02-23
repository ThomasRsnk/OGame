using System;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Services;
using Djm.OGame.Web.Api.Services.Pictures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/api/universes/{universeId:int}/players/{playerId:int}/[Controller]")]
    public class ProfilePicController : Controller
    {
        public ProfilePicController(IPicture picture)
        {
            Picture = picture;
        }

        internal IPicture Picture { get; }


        [HttpPost]
        public async Task<IActionResult> AddProfilePic(int universeId,int playerId, IFormFile pic)
        {
            try
            {
                await Picture.Set(universeId, playerId, pic);
            }
            catch (PictureException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult GetProfilePic(int universeId, int playerid)
        {
            var image = Picture.Get(universeId, playerid);

            if (image == null)
                return NotFound("Le joueur "+playerid+" n'existe pas");

            var extension = image.Name.Substring(image.Name.LastIndexOf(".", StringComparison.Ordinal) + 1);
            var contentType = "image/" + extension;

            return File(image, contentType);
        }



        
    }
}