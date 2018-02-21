using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OGame.Client;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("~/api/universes/{universeId:int}/players/[Controller]/{playerId:int}")]
    public class ProfilePicController : Controller
    {
        public ProfilePicController(IOgClient ogclient)
        {
            OgameClient = ogclient;
        }

        internal IOgClient OgameClient { get; set; }

        [HttpPost]
        public async Task<IActionResult> AddProfilePic(IFormFile pic,int universeId,int playerId)
        {
            var playerIdStr = playerId.ToString("D", CultureInfo.InvariantCulture);
            var universeIdStr = universeId.ToString("D", CultureInfo.InvariantCulture);
            var fileExtension = "." + pic.ContentType.Substring(pic.ContentType.IndexOf("/", StringComparison.Ordinal)+1);
            var path = "wwwroot/profilePictures/" + universeIdStr+"/";
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            
            var files = Directory.GetFiles(path, playerIdStr+".*");
            
            if(files.Length>0)
                System.IO.File.Delete(files[0]);

            path += playerIdStr + fileExtension;

            using (var fileStream = System.IO.File.Create(path))
            {
                await pic.CopyToAsync(fileStream); 
            }

            

            return Ok(pic.ContentType);
        }



        
    }
}