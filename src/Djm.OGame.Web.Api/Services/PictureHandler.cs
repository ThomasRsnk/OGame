using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services
{
    public class PictureHandler : IPictureResource
    {
        public IOgClient OGameClient { get; }
        private const string BasePath = "wwwroot/profilePictures/";
        private List<string> AllowedFileType {get;}
        

        public PictureHandler(IOgClient oGameClient)
        {
            OGameClient = oGameClient;
            AllowedFileType = new List<string>()
            {
                "image/jpeg","image/png","image/bmp","image/jpg"
            };
        }

        public async Task Set(int universeId, int playerId, IFormFile pic)
        {
            //vérifier que le type de fichier est valide

            if(!AllowedFileType.Contains(pic.ContentType))
                throw  new PictureException("Fichier invalide, utilisez .png/.jpeg/.bmp");

            //vérifier que l'univers et le joueur existent

            var players = OGameClient.Universe(universeId).GetPlayers();
            if(players==null)
                throw new PictureException("L'univers "+universeId+" n'existe pas");
            if (!players.Exists(p=>p.Id == playerId))
                throw new PictureException("Le joueur "+playerId+" n'existe pas");

            //récupérer les ids & le type de fichier

            var playerIdStr = playerId.ToString("D", CultureInfo.InvariantCulture);
            var universeIdStr = universeId.ToString("D", CultureInfo.InvariantCulture);
            var fileExtension = "." + pic.ContentType.Substring(pic.ContentType.IndexOf("/", StringComparison.Ordinal) + 1);

            //vérifier que le répertoire de l'univers spéficié existe

            var path = BasePath + universeIdStr + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //vérifier si image déjà présente (peu importe jpg,png,etc) et suppression le cas échéant

            var files = Directory.GetFiles(path, playerIdStr + ".*");

            if (files.Length > 0)
                File.Delete(files[0]);

            //enregistrer l'image
            path += playerIdStr + fileExtension;

            using (var fileStream = File.Create(path))
            {
                await pic.CopyToAsync(fileStream);
            }

            
        }

        public FileStream Get(int universeId, int playerId)
        {
            //récupérer les ids 
            var playerIdStr = playerId.ToString("D", CultureInfo.InvariantCulture);
            var universeIdStr = universeId.ToString("D", CultureInfo.InvariantCulture);

            var path = BasePath + universeIdStr + "/";
            var files = Directory.GetFiles(path, playerIdStr + ".*");

            return files.Length > 0 ? File.OpenRead(files[0]) : null;
        }
    }
}