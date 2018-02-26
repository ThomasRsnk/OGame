using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IPictureResource
    {
        Task<bool> Set(int playerId,string path,CancellationToken ct);
        string Get(int playerId);
    }
}