using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Djm.OGame.Web.Api.Services.OGame.Pictures
{
    public interface IPictureService
    {
        Task SavePictureAsync(string email, IFormFile pic,CancellationToken cancellation = default(CancellationToken));
        Task<FileStream> GetAsync(string email, CancellationToken cancellation = default(CancellationToken));
    }
}