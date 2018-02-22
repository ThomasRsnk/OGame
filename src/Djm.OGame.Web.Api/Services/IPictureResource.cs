using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Djm.OGame.Web.Api.Services
{
    public interface IPictureResource
    {
        Task Set(int universeId, int playerId, IFormFile pic);
        FileStream Get(int universeId, int playerId);
    }
}