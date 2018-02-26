using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Http;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.Pictures
{
    public class PictureHandler : IPicturehandler
    {
        public IOgClient OGameClient { get; }
        public IUnitOfWork UnitOfWork { get; }
        private readonly string _basePath = Path.Combine("wwwroot", "profilePictures");
        private List<string> AllowedFileType {get;}
        

        public PictureHandler(IOgClient oGameClient,IUnitOfWork unitOfWork)
        {
            OGameClient = oGameClient;
            UnitOfWork = unitOfWork;
            AllowedFileType = new List<string>()
            {
                "image/jpeg","image/png","image/bmp","image/jpg"
            };
        }

        public async Task SavePictureAsync(int universeId, int playerId, IFormFile pic)
        {
            //vérifier que le fichier a bien été reçu

            if(pic == null)
                throw new PictureException("Error while uploading file");

            //vérifier que le type de fichier est valide

            if(!AllowedFileType.Contains(pic.ContentType))
                throw  new PictureException("Fichier invalide, utilisez .png/.jpeg/.bmp/.jpg");

            //vérifier que l'univers et le joueur existent

            var players = OGameClient.Universe(universeId).GetPlayers();
            if(players==null)
                throw new PictureException("L'univers "+universeId+" n'existe pas");
            if (!players.Exists(p=>p.Id == playerId))
                throw new PictureException("Le joueur "+playerId+" n'existe pas");

            //conversion des id en str, créer le nom du fichier & récupération du répertoire de l'univers

            var playerIdStr = Utils.Utils.ToStringInvariant(playerId);
            var universeIdStr = Utils.Utils.ToStringInvariant(universeId);

            var fileName = playerIdStr+ "." + pic.ContentType.Substring(pic.ContentType.IndexOf("/", StringComparison.Ordinal) + 1);
            
            var path = Path.Combine(_basePath, universeIdStr);
           
            //vérifier si l'image est déjà présente

            var player = await UnitOfWork.Players.FirstOrDefaultAsync(universeId, playerId);

            if (player != null)//OUI
            {
                // 1) Modification en db

                player.ProfilePicturePath = fileName;
                UnitOfWork.Players.Update(player);
                
                // 2) Suppression dans le fs
                
                var files = Directory.GetFiles(path, playerIdStr + ".*");

                foreach(var file in files)
                    File.Delete(file);
            }
            else//NON
            {
                // 1) vérifier que le répertoire de l'univers spéficié existe

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // 2) création d'un tuple en db

                UnitOfWork.Players.Insert(new Player()
                {
                    Id = playerId,
                    UniverseId = universeId,
                    ProfilePicturePath = fileName
                });
            }

            //enregistrer l'image dans le fs et commit
            path = Path.Combine(path, fileName);

            using (var fileStream = File.Create(path))
            {
                await pic.CopyToAsync(fileStream);
            }

            await UnitOfWork.CommitAsync();
        }

        public async Task<FileStream> GetAsync(int universeId, int playerId)
        {
            var player = await UnitOfWork.Players.FirstOrDefaultAsync(universeId, playerId);

            var universeIdStr = Utils.Utils.ToStringInvariant(universeId);

            return player != null ? File.OpenRead(Path.Combine(_basePath,universeIdStr,player.ProfilePicturePath)) : null;
        }
    }
}