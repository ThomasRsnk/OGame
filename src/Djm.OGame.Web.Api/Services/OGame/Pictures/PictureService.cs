using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Player;
using Djm.OGame.Web.Api.Dal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Pictures
{
    public class PictureService : IPictureService
    {
        public IOgClient OGameClient { get; }
        public IUnitOfWork UnitOfWork { get; }
        public UserManager<ApplicationUser> UserManager { get; }

        private readonly string _basePath = Path.Combine("wwwroot", "profilePictures");
        private List<string> AllowedFileType {get;}
        

        public PictureService(IOgClient oGameClient,IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            OGameClient = oGameClient;
            UnitOfWork = unitOfWork;
            UserManager = userManager;
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

            //vérifier que l'utilisateur existe

            var user = await UserManager.FindByEmailAsync(email);

            if(user == null)
                throw new PictureException("L'utilisateur n'existe pas");

            // créer le nom du fichier 
            
            var fileName = user.Id+ "." + pic.ContentType.Substring(pic.ContentType.IndexOf("/", StringComparison.Ordinal) + 1);
            
            var path = Path.GetFullPath(Path.Combine(_basePath,fileName));
            
            //vérifier si l'image est déjà présente
            
            if (user.ProfilePicturePath != "")//OUI
            {
                var files = Directory.GetFiles(_basePath, user.Id + ".*");

                foreach(var file in files)
                    File.Delete(file);
            }
            
            //resize de l'image et enregistrement dans le fs

            using (var ms = new MemoryStream())
            {
                pic.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);

                var i = new ImageResizer.ImageJob(ms, path, new ImageResizer.Instructions(
                    "width=256;height=256;format=jpg;mode=max"));
                i.Build();
            }

            //maj en db et commit

            user.ProfilePicturePath = path;

            await UserManager.UpdateAsync(user);
        }

        public async Task<FileStream> GetAsync(string email,CancellationToken cancellation)
        {
            var user = await UserManager.FindByEmailAsync(email);

            return user != null ? File.OpenRead(user.ProfilePicturePath) : null;
        }
    }
}