using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Djm.OGame.Web.Api.BindingModels.Universes;
using Djm.OGame.Web.Api.Dal.Entities;
using Djm.OGame.Web.Api.Dal.Repositories.Univers;
using Djm.OGame.Web.Api.Dal.Services;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services.OGame.Universes
{
    public interface IUniversService
    {
        Task<List<UniverseListItemViewModel>> GetUniverses(CancellationToken ct = default(CancellationToken));
    }

    public class UniverseService : IUniversService
    {
        public IUnitOfWork UnitOfWork { get; }
        public IOgClient OgameClient { get; }
        public IMapper Mapper { get; }
        public IUniversRepository UniversRepository { get; }


        public UniverseService(IUnitOfWork unitOfWork, IOgClient ogClient, IMapper mapper,IUniversRepository universRepository)
        {
            UnitOfWork = unitOfWork;
            OgameClient = ogClient;
            Mapper = mapper;
            UniversRepository = universRepository;
        }

        public async Task<List<UniverseListItemViewModel>> GetUniverses(CancellationToken ct = default(CancellationToken))
        {
            var uni = await UniversRepository.ToListAsync(ct);
            
            if (uni.Any()) return Mapper.Map<List<UniverseListItemViewModel>>(uni);

            var universes = OgameClient.Universe(1).GetUniverses();

            if (universes == null)
                return null;

            foreach (var entry in universes)
                UniversRepository.Insert(new Univers() { Id = entry.Id, Name = entry.Name });

            await UnitOfWork.CommitAsync(ct);

            return universes;

        }
    }

}