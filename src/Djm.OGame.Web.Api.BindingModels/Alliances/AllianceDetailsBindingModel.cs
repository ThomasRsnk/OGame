using System;
using System.Collections.Generic;
using Djm.OGame.Web.Api.BindingModels.Players;
using Djm.OGame.Web.Api.BindingModels.Scores;

namespace Djm.OGame.Web.Api.BindingModels.Alliances
{
    public class AllianceDetailsBindingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public DateTime FoundDate { get; set; }
        public PlayerListItemBindingModel Founder { get; set; }
        public string Logo { get; set; }
        public string HomePage { get; set; }
        public ScoreBaseBindingModel Score { get; set; }
        public List<PlayerListItemBindingModel> Members { get; set; }
    }
}
