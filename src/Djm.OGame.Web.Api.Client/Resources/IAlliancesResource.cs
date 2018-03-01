using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.BindingModels.Pagination;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IAlliancesResource
    {
        Task<PagedListViewModel<AllianceListItemBindingModel>> GetAllAsync(int page = 0, int pageLength = 50, CancellationToken cancellationToken = default(CancellationToken));
        Task<AllianceDetailsBindingModel> GetDetailsAsync(int allianceId, CancellationToken cancellationToken = default(CancellationToken));
    }
}