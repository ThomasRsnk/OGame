using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Services
{
    public interface IPictureResource
    {
        Task Set(int universeId, int playerId, IFormFile pic);
        FileStream Get(int universeId, int playerId);
    }
}