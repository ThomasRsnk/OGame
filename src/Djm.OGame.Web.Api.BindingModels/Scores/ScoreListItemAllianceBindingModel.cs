using Djm.OGame.Web.Api.BindingModels.Alliances;

namespace Djm.OGame.Web.Api.BindingModels.Scores
{
    public class ScoreListItemAllianceBindingModel
    {
        public AllianceListItemBindingModel Alliance { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }
    }
}