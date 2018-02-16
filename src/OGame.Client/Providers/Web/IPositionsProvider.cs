using System.Collections.Generic;
using OGame.Client.Models;

namespace OGame.Client.Providers.Web
{
    internal interface IPositionsProvider : IEntityProvider<int, List<Position>>
    {
    }
}