using Djm.OGame.Web.Api.Dal.Resources;

namespace Djm.OGame.Web.Api.Dal
{
    public class OgameDb : IOgameDb
    {
        public OgameDb()
        {
            Ctx = new OGameContext();

            Pins = new PinResourceDb(Ctx);
        }

        private OGameContext Ctx { get; }

        public IPinResource Pins { get; }
    }
}