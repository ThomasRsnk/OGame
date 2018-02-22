using System.Collections.Generic;
using OGame.Client;
using OGame.Client.Models;

namespace Djm.OGame.Web.Api.Services
{
    public interface IOgameService
    {
        List<Player> GetPlayers(int universe);
        List<Alliance> GetAlliances(int universe);
        List<PlayerScore> GetPlayersScores(int universe,int type);
        List<AllianceScore> GetAlliancesScores(int universe);
        Player GetPlayerData(int playerId, int universe);
        bool Connect(int universe, string name);
        Alliance GetAllianceDetails(int universe, int id);
        Dictionary<int, string> GetUniverses(int serverId);
    }

    public class OgameFromClient : IOgameService
    {
        public IOgClient OgClient { get; }

        private readonly Dictionary<int, IOgUniverseClient> _universes = new Dictionary<int, IOgUniverseClient>();

        public OgameFromClient(IOgClient ogClient)
        {
            OgClient = ogClient;
        }

        private IOgUniverseClient GetInstance(int universeId)
        {
            if (!_universes.ContainsKey(universeId))
                _universes.Add(universeId, OgClient.Universe(universeId));

            return _universes[universeId];
        }

        public List<Player> GetPlayers(int universe)
        {
            return GetInstance(universe).GetPlayers();
        }

        public List<Alliance> GetAlliances(int universe)
        {
            return GetInstance(universe).GetAlliances();
        }

        public List<PlayerScore> GetPlayersScores(int universe, int type)
        {
            return GetInstance(universe).GetPlayersScores(type);
        }

        public List<AllianceScore> GetAlliancesScores(int universe)
        {
            return GetInstance(universe).GetAllianceScores();
        }

        public Player GetPlayerData(int playerId, int universe)
        {
            return GetInstance(universe).GetPlayer(playerId);
        }

        public bool Connect(int universe, string name)
        {
            return GetInstance(universe).GetPlayers().Exists(p => p.Name == name);
        }

        public Alliance GetAllianceDetails(int universe, int allianceId)
        {
            return GetInstance(universe).GetAlliance(allianceId);
        }

        public Dictionary<int, string> GetUniverses(int serverId)
        {
            return GetInstance(serverId).GetUniverses();
        }
    }
}
