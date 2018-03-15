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

        public async Task SavePictureAsync(string email, IFormFile pic,CancellationToken cancellation)
        {
            //vérifier que le fichier a bien été reçu

            if(pic == null)
                throw new PictureException("Error while uploading file");

            //vérifier que le type de fichier est valide

            if(!AllowedFileType.Contains(pic.ContentType))
                throw  new PictureException("Fichier invalide, utilisez .png/.jpeg/.bmp/.jpg");

            //vérifier que le joueur existe

            var player = await PlayerRepository.FirstOrDefaultAsync(email, cancellation);

            if(player == null)
                throw new PictureException("Le joueur n'existe pas");

            // créer le nom du fichier & récupération du répertoire de l'univers
            
            var fileName = player.OGameId+ "." + pic.ContentType.Substring(pic.ContentType.IndexOf("/", StringComparison.Ordinal) + 1);

            var directory = Path.Combine(_basePath, player.UniverseId.ToString());

            var path = Path.GetFullPath(Path.Combine(directory,fileName));
            
            //vérifier si l'image est déjà présente
            
            if (player.ProfilePicturePath != "")//OUI
            {
                var files = Directory.GetFiles(directory, player.OGameId + ".*");

                foreach(var file in files)
                    File.Delete(file);
            }
            else//NON
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            //maj en db

            player.ProfilePicturePath = path;
            PlayerRepository.Update(player);


            //enregistrer l'image dans le fs et commit

            using (var fileStream = File.Create(path))
            {
                await pic.CopyToAsync(fileStream, cancellation);
            }

            await UnitOfWork.CommitAsync(cancellation);
        }

        public async Task<FileStream> GetAsync(string email,CancellationToken cancellation)
        {
            var player = await PlayerRepository.FirstOrDefaultAsync(email, cancellation);

            return player != null ? File.OpenRead(Path.Combine(player.ProfilePicturePath)) : null;
        }
    }
}