using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Universes;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IUniversesResource
    {
        IUniverseResource this[int universeId] { get; }

        Task<List<UniverseListItemViewModel>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}