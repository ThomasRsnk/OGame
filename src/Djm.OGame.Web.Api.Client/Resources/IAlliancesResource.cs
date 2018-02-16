using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Alliances;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IAlliancesResource
    {
        Task<List<AllianceListItemBindingModel>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<AllianceDetailsBindingModel> GetDetailsAsync(int allianceId, CancellationToken cancellationToken = default(CancellationToken));
    }
}