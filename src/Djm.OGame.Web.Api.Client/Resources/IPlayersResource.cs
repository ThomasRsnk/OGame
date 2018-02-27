using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Players;

namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IPlayersResource
    {
        Task<List<PlayerListItemBindingModel>> GetAllAsync(int skip=0,int take = Int32.MaxValue,CancellationToken cancellationToken = default(CancellationToken));
        Task<PlayerDetailsBindingModel> GetDetailsAsync(int playerId, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Connect(string playerName, CancellationToken cancellationToken = default(CancellationToken));
    }
}