using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Djm.OGame.Web.Api.BindingModels.Universes;
using Microsoft.Extensions.Options;
using OGame.Client.Models;
using OGame.Client.Providers.Cache;
using OGame.Client.Providers.Log;
using OGame.Client.Providers.Web;
using OGame.Client.XmlBinding;

namespace OGame.Client
{
    public class OgUniverseClient : IAllianceProvider, IPlayerProvider, IScoreProvider, IPlanetProvider, IOgUniverseClient,IPositionsProvider
    {
        public OgUniverseClient(int id)
        {
            Id = id;
            BaseUrl = "http://s" + id.ToString("D", CultureInfo.InvariantCulture) + "-fr.ogame.gameforge.com/api/";
            
            AllianceCache = new CacheAllianceProvider(new AllianceLogProvider("alliance cache", this));
            PlayerCache = new CachePlayerProvider(new PlayerLogProvider("players cache", this));

            AllianceProvider = new AllianceLogProvider("alliance", AllianceCache);
            PlayerProvider = new PlayerLogProvider("players", PlayerCache);
            ScoreProvider = this;
            PlanetProvider = this;
            PositionsProvider = this;
        }

        public int Id { get; }
        public string BaseUrl { get; }

        internal CacheAllianceProvider AllianceCache { get; }
        internal CachePlayerProvider PlayerCache { get; }
        

        internal IAllianceProvider AllianceProvider { get; }
        internal IPlayerProvider PlayerProvider { get; }
        internal IScoreProvider ScoreProvider { get; }
        internal IPlanetProvider PlanetProvider { get; }
        internal IPositionsProvider PositionsProvider { get; }

        internal List<int> UniversesIds { get; set; }

        public List<Player> GetPlayers()
        {
            var url = BaseUrl + "players.xml";
            
            var playersXml = Deserialize<PlayersXmlBinding>(url);

            if (playersXml == null) return null;

            var players = playersXml.ListPlayers.Select(p => new Player(AllianceProvider, PositionsProvider, PlanetProvider)
            {
                Id = p.Id,
                AllianceId = p.AllianceId,
                Name = p.Name,
                Status = Player.StatusFromString(p.Status)
            }).ToList();

            foreach (var player in players)
                PlayerCache.Populate(player.Id, player);

            return players;
        }

        
        public List<Alliance> GetAlliances()
        {
            var url = BaseUrl + "alliances.xml";
            var alliancesXml = Deserialize<AlliancesXmlBinding>(url);

            if (alliancesXml == null) return null;

            var alliances = alliancesXml.ListAlliances.Select(a => new Alliance(PlayerProvider,ScoreProvider)
            {
                Id = a.Id,
                Name = a.Name,
                MemberIds = a.Members.Select(m => m.Id).ToList(),
                FounderId = a.Founder,
                FoundDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(a.FoundDate),
                Tag = a.Tag,
                Logo = a.Logo,
                HomePage = a.HomePage,
            }).ToList();

            foreach (var alliance in alliances)
                AllianceCache.Populate(alliance.Id, alliance);
            
            return alliances;
        }

        public List<PlayerScore> GetPlayersScores(int type)
        {
            var url = BaseUrl + "highscore.xml?category=1&type=" + type.ToString("D", CultureInfo.InvariantCulture);
            var scoresXml = Deserialize<HighscoresXmlBinding>(url);

            var scores = scoresXml?.Scores.Select(s => new PlayerScore(PlayerProvider)
            {
                Points = s.ScoreTotal,
                Rank = s.Position,
                Id = s.Id,
            }).ToList();

            return scores;
        }


        public List<AllianceScore> GetAllianceScores()
        {
            var url = BaseUrl + "highscore.xml?category=2&type=0";
            var scoresXml = Deserialize<HighscoresXmlBinding>(url);

            var scores = scoresXml?.Scores_A.Select(s => new AllianceScore(AllianceProvider)
            {
                Points = s.ScoreTotal,
                Rank = s.Position,
                Id = s.Id
            }).ToList();

            return scores;
        }

        public List<Planet> GetPlanets()
        {
            var url = BaseUrl + "universe.xml";
            var planetsXml = Deserialize<UniversePlanetXmlBinding>(url);

            var planets = planetsXml?.Planets.Select(p => new Planet(PlayerProvider)
            {
                Id = p.Id,
                Coords = p.Coords,
                PlayerId = p.PlayerId,
                Name = p.Name,
                Moon = p.Lune != null ? new Moon(p.Lune.Id, p.Lune.Name, p.Lune.Size) : null
            }).ToList();

            return planets;
        }

        public List<Planet> GetPlanetsForPlayer(int playerId)
        {
            var url = BaseUrl + "playerData.xml?id=" + playerId;
            var planetsXml = Deserialize<PlanetsXmlBinding>(url);
            
            var planets = planetsXml?.Planets.Select(p => new Planet(PlayerProvider)
            {
                Id = p.Id,
                Coords = p.Coords,
                Name = p.Name,
                Moon = p.Lune != null ? new Moon(p.Lune.Id, p.Lune.Name, p.Lune.Size) : null
            }).ToList();

            return planets;
        }

        public Player GetPlayer(int playerId) 
            => GetPlayers().FirstOrDefault(p => p.Id == playerId);

        public Alliance GetAlliance(int allianceId) 
            => GetAlliances().FirstOrDefault(a => a.Id == allianceId);

        public Planet GetPlanet(int planetId)
            => GetPlanets().FirstOrDefault(p => p.Id == planetId);

        private void GetUniversesIds()
        {
            UniversesIds = new List<int>();

            var url = BaseUrl + "universes.xml";
            var universesXml = Deserialize<UniversesXmlBinding>(url);

            if (universesXml == null) return;

            UniversesIds = universesXml.Univers.Select(u => u.Id).ToList();
        }

        public List<UniverseListItemViewModel> GetUniverses()
        {
            GetUniversesIds();
            var universes = new List<UniverseListItemViewModel>();
            foreach (var n in UniversesIds)
            {
                var url = "http://s" + n.ToString("D",CultureInfo.InvariantCulture) + "-fr.ogame.gameforge.com/api/serverData.xml";
                var serverDataXml = Deserialize<ServerDataXmlBinding>(url);
                
                universes.Add(new UniverseListItemViewModel()
                {
                    Id = serverDataXml.Id,
                    Name = serverDataXml.Name ?? "OGame "+serverDataXml.Id
                });
            }

            return universes;
        }

        public bool Exists(int playerId)
        {
            return GetPlayers().Exists(p => p.Id == playerId);
        }

        public List<Position> GetPositions(int playerId)
        {
            var url = BaseUrl + "playerData.xml?id=" + playerId;
            var positionsXml = Deserialize<PositionsXmlBindingModel>(url);

            var positions = positionsXml?.Positions.Select(p => new Position()
            {
                Score = p.Score,
                Type = p.Type,
                Rank = p.Position,
                TypeC = (Position.TypeClassement) p.Type,
                Ships = p.Ships
            }).ToList();

            return positions;
        }
        

        private static T Deserialize<T>(string url)
        {
            var serializer = new XmlSerializer(typeof(T));

            try
            {
                using (var reader = XmlReader.Create(url))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (WebException)
            {
                return default(T);
            }
            
        }

        Alliance IEntityProvider<int, Alliance>.Get(int id)
            => GetAlliance(id) ?? new Alliance(PlayerProvider,ScoreProvider);

        Player IEntityProvider<int, Player>.Get(int id)
            => GetPlayer(id) ?? new Player(AllianceProvider, PositionsProvider, PlanetProvider);

        List<Position> IEntityProvider<int, List<Position>>.Get(int id)
            => GetPositions(id);   

        Planet IEntityProvider<int, Planet>.Get(int id) 
            => GetPlanet(id) ?? new Planet(PlayerProvider);

        List<Planet> IPlanetProvider.GetPlanetsForPlayer(int playerId)
            => GetPlanetsForPlayer(playerId);

        AllianceScore IEntityProvider<int, AllianceScore>.Get(int allianceId)
            => GetAllianceScores().FirstOrDefault(s => s.Id == allianceId);
        
    }

    public class OgClient : IOgClient
    {
        private readonly Dictionary<int, IOgUniverseClient> _universes = new Dictionary<int, IOgUniverseClient>();
        
        public IOgUniverseClient Universe(int universeId)
        {
            if (!_universes.ContainsKey(universeId))
                _universes.Add(universeId, new OgUniverseClient(universeId));

            return _universes[universeId];
        }
        
    }
}
