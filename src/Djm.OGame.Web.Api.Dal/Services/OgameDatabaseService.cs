using System.Threading.Tasks;
using Djm.OGame.Web.Api.Dal.Repositories;

namespace Djm.OGame.Web.Api.Dal.Services
{
    public class OgameDatabaseService : IOgameDatabaseService
    {
        public OgameDatabaseService(IPinRepository pins)
        {
            Pins = pins;
        }

        public IPinRepository Pins { get; }

        public async Task SaveChangesAsync()
        {
            await Pins.SaveChangesAsync();
            //await Abc.SaveChangesAsync();
        }
    }
}