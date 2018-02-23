using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Djm.OGame.Web.Api.Services.Pictures
{
    public interface IPicture
    {
        Task Set(int universeId, int playerId, IFormFile pic);
        FileStream Get(int universeId, int playerId);
    }
}