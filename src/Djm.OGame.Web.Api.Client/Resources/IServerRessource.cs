using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IServersResource
    {
        Task<Dictionary<int, string>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}