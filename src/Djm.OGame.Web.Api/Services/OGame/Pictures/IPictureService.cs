using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Djm.OGame.Web.Api.Services.OGame.Pictures
{
    public interface IPictureService
    {
        Task SavePictureAsync(int universeId, int playerId, IFormFile pic,CancellationToken cancellation = default(CancellationToken));
        Task<FileStream> GetAsync(int universeId, int playerId, CancellationToken cancellation = default(CancellationToken));
    }
}