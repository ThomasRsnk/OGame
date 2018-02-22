using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories;

namespace Djm.OGame.Web.Api.Dal.Services
{
    public interface IOgameDatabaseService
    {
        IPinRepository Pins { get; }

        Task SaveChangesAsync();
    }
}