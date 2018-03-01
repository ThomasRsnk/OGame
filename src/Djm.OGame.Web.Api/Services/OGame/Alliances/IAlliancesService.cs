using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.BindingModels.Pagination;

namespace Djm.OGame.Web.Api.Services.OGame.Alliances
{
    public interface IAlliancesService
    {
        PagedListViewModel<AllianceListItemBindingModel> GetAll(int universeId, Page page);

        AllianceDetailsBindingModel GetDetails(int universeId, int allianceId);

    }
}