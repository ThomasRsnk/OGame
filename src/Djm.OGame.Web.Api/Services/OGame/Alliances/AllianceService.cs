using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Alliances
{
    public class AllianceService : IAlliancesService
    {
        public IOgClient OgameClient { get; }
        public IMapper Mapper { get; }

        public AllianceService(IOgClient ogClient, IMapper mapper)
        {
            OgameClient = ogClient;
            Mapper = mapper;
        }
        
        public PagedListViewModel<AllianceListItemBindingModel> GetAll(int universeId, Page page)
        {
            var alliances = OgameClient.Universe(universeId).GetAlliances();

            if (alliances == null) throw new OGameException("L'univers "+universeId+" n'existe pas");

            var viewModel = Mapper.Map<List<AllianceListItemBindingModel>>(alliances);

            return viewModel.ToPagedListViewModel(page);
        }

        public AllianceDetailsBindingModel GetDetails(int universeId, int allianceId)
        {
            var alliances = OgameClient.Universe(universeId).GetAlliances();

            if (alliances == null) throw new OGameException("Cet univers n'existe pas");

            var alliance = alliances.FirstOrDefault(a => a.Id == allianceId);

            if (alliance == null) throw new OGameException("Cette alliance n'existe pas");

            var viewModel = Mapper.Map<AllianceDetailsBindingModel>(alliance);

            return viewModel;
        }
    }
}