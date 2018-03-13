using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Djm.OGame.Web.Api.Helpers;
using Microsoft.AspNetCore.Http;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Pictures
{
    public class PictureService : IPictureService
    {
        public IOgClient OGameClient { get; }
        public IUnitOfWork UnitOfWork { get; }
        public IPlayerRepository PlayerRepository { get; }
        private readonly string _basePath = Path.Combine("wwwroot", "profilePictures");
        private List<string> AllowedFileType {get;}
        

        public PictureService(IOgClient oGameClient,IUnitOfWork unitOfWork,IPlayerRepository playerRepository)
        {
            OGameClient = oGameClient;
            UnitOfWork = unitOfWork;
            PlayerRepository = playerRepository;
            AllowedFileType = new List<string>()
            {
                "image/jpeg","image/png","image/bmp","image/jpg"
            };
        }

        public async Task SavePictureAsync(int universeId, int playerId, IFormFile pic,CancellationToken cancellation)
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

            var playerIdStr = Utils.ToStringInvariant(playerId);
            var universeIdStr = Utils.ToStringInvariant(universeId);

            var fileName = playerIdStr+ "." + pic.ContentType.Substring(pic.ContentType.IndexOf("/", StringComparison.Ordinal) + 1);

            var directory = Path.Combine(_basePath, universeIdStr);

            var path = Path.GetFullPath(Path.Combine(directory,fileName));
            
           
            //vérifier si l'image est déjà présente

            var player = await PlayerRepository.FirstOrDefaultAsync(universeId, playerId, cancellation);

            if (player != null)//OUI
            {
                // 1) Modification en db

                player.ProfilePicturePath = path;
                PlayerRepository.Update(player);
                
                // 2) Suppression dans le fs
                
                var files = Directory.GetFiles(directory, playerIdStr + ".*");

                foreach(var file in files)
                    File.Delete(file);
            }
            else//NON
            {
                // 1) vérifier que le répertoire de l'univers spéficié existe

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // 2) création d'un tuple en db

                PlayerRepository.Insert(new Player()
                {
                    Id = playerId,
                    UniverseId = universeId,
                    ProfilePicturePath = path
                });
            }

            //enregistrer l'image dans le fs et commit

            using (var fileStream = File.Create(path))
            {
                await pic.CopyToAsync(fileStream, cancellation);
            }

            await UnitOfWork.CommitAsync(cancellation);
        }

        public async Task<FileStream> GetAsync(int universeId, int playerId,CancellationToken cancellation)
        {
            var player = await PlayerRepository.FirstOrDefaultAsync(universeId, playerId, cancellation);

            return player != null ? File.OpenRead(Path.Combine(player.ProfilePicturePath)) : null;
        }
    }
}