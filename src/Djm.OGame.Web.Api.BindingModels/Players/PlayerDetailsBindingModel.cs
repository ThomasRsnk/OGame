using System.Collections.Generic;
using Djm.OGame.Web.Api.BindingModels.Alliances;
using Djm.OGame.Web.Api.BindingModels.Pins;
using Djm.OGame.Web.Api.BindingModels.Planets;
using Djm.OGame.Web.Api.BindingModels.Scores;

namespace Djm.OGame.Web.Api.BindingModels.Players
{
    public class PlayerDetailsBindingModel : PlayerListItemBindingModel
    {
        public List<PinListItemBindingModel> Favoris { get; set; }
        public List<PlanetBindingModel> Planets { get; set; }
        public List<PositionsBindingModel> Positions { get; set; }
        public AllianceListItemBindingModel Alliance { get; set; }
        public string Status { get; set; }
    }
}