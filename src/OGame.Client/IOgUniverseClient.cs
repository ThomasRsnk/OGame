using System;
using System.Collections.Generic;
using Djm.OGame.Web.Api.BindingModels.Universes;
using OGame.Client.Models;
using OGame.Client.XmlBinding;

namespace OGame.Client
{
    public interface IOgUniverseClient
    {
        string BaseUrl { get; }
        int Id { get; }

        Alliance GetAlliance(int allianceId);
        List<Alliance> GetAlliances();
        Planet GetPlanet(int planetId);
        List<Planet> GetPlanets();
        Player GetPlayer(int playerId);
        List<Player> GetPlayers();
        List<PlayerScore> GetPlayersScores(int type);
        List<AllianceScore> GetAllianceScores();
        List<Position> GetPositions(int playerId);
        List<UniverseListItemViewModel> GetUniverses();
    }
}