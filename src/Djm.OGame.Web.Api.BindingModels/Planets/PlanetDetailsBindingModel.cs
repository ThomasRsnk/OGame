using Djm.OGame.Web.Api.BindingModels.Players;

namespace Djm.OGame.Web.Api.BindingModels.Planets
{
    public class PlanetDetailsBindingModel  
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coords { get; set; }
        public MoonBindingModel Moon { get; set; }
        public PlayerListItemBindingModel Player { get; set; }
    }
}
