using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.Dal;
using Djm.OGame.Web.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/api/universes/{universeId:int}/players/{playerId:int}/[Controller]")]
    public class ProfilePicController : Controller
    {
        public ProfilePicController(IPictureResource pictureResource)
        {
            PictureResource = pictureResource;
        }

        internal IPictureResource PictureResource { get; }


        [HttpPost]
        public async Task<IActionResult> AddProfilePic(int universeId,int playerId, IFormFile pic)
        {
            try
            {
                await PictureResource.Set(universeId, playerId, pic);
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
            var image = PictureResource.Get(universeId, playerid);

            if (image == null)
                return NotFound("Le joueur "+playerid+" n'existe pas");

            var extension = image.Name.Substring(image.Name.LastIndexOf(".", StringComparison.Ordinal) + 1);
            var contentType = "image/" + extension;

            return File(image, contentType);
        }



        
    }
}