using Djm.OGame.Web.Api.BindingModels.Players;

namespace Djm.OGame.Web.Api.BindingModels.Scores
{
    public class ScoreListItemPlayerBindingModel 
    {
        public PlayerListItemBindingModel Player { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }

    }
}