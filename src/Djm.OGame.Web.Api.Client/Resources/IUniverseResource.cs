﻿namespace Djm.OGame.Web.Api.Client.Resources
{
    public interface IUniverseResource
    {
        IPlayersResource Players { get; }
        IAlliancesResource Alliances { get; }
        IScoresResource Scores { get; }
        IPinsResource Pins { get; }
        IPictureResource Pictures { get; }
    }
}