using System;
using System.Collections.Generic;
using System.Linq;
using Djm.OGame.Web.Api.Dal;
using OGame.Client.Providers.Web;

namespace OGame.Client.Models
{
    public class Player
    {
        internal IAllianceProvider AllianceProvider { get; }
        internal IPositionsProvider PositionsProvider { get; }
        internal IPlanetProvider PlanetProvider { get; }
        internal IPlayerProvider PlayerProvider { get; }

        internal Player(IAllianceProvider allianceProvider, IPositionsProvider positionsProvider, IPlanetProvider planetprovider,IPlayerProvider pP)
        {
            AllianceProvider = allianceProvider;
            PositionsProvider = positionsProvider;
            PlanetProvider = planetprovider;
            PlayerProvider = pP;

            OgameDbProvider = new OgameDb();
        }

        internal IOgameDb OgameDbProvider { get; }

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        
        public int? AllianceId { get; internal set; }
        public Alliance Alliance => AllianceId == null ? null : AllianceProvider.Get(AllianceId.Value);

        public List<Position> Positions => PositionsProvider.Get(Id);

        public IEnumerable<int> PlanetIds { get; internal set; }
        public List<Planet> Planets => PlanetProvider.GetPlanetsForPlayer(Id);
      
        public PlayerStatus? Status { get; internal set; }
        public bool IsAdministrator => Status?.HasFlag(PlayerStatus.Administrator) == true;

        public List<Favori> Favoris
        {
            get
            {
                var pins = OgameDbProvider.Pins.ToList(Id);//pins
                
                if (!pins.Any()) return new List<Favori>();

                var pinsIds = pins.Select(p => p.TargetId).ToList();//liste des id des joueurs ciblés

                var players =  pinsIds //liste des joueurs
                    .Select(id => PlayerProvider.Get(id))
                    .ToList();

                var favoris = players //liste des favoris
                    .Join(pins, player => player.Id, pin => pin.TargetId, (player, pin)
                        => new Favori() { Id = pin.Id, PlayerId = player.Id, Name = player.Name }).ToList();

                return favoris;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Status)}: {Status}, {nameof(AllianceId)}: {AllianceId}";
        }

        public static PlayerStatus? StatusFromString(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
                return null;

            var status = PlayerStatus.None;

            if (str.Contains("a"))
                status |= PlayerStatus.Administrator;
            if (str.Contains("I"))
                status |= PlayerStatus.Inactif_depuis_28_jours;
            if (str.Contains("i"))
                status |= PlayerStatus.Inactif_depuis_7_jours;
            if (str.Contains("v"))
                status |= PlayerStatus.Mode_vacances;
            if (str.Contains("b"))
                status |= PlayerStatus.Blocked;
            if (str.Contains("f"))
                status |= PlayerStatus.Joueur_fort;
            if (str.Contains("d"))
                status |= PlayerStatus.Joueur_faible;
            if (str.Contains("b"))
                status |= PlayerStatus.Blocked;
            if (str.Contains("o"))
                status |= PlayerStatus.Outlaw;
            if (str.Contains("ph"))
                status |= PlayerStatus.Cible_honorable;

            return status;
        }
    }

    [Flags]
    public enum PlayerStatus
    {
        None = 0,

        Administrator = 1 << 0,
        Joueur_fort = 1 << 1,
        Joueur_faible = 1 << 2,
        Inactif_depuis_28_jours = 1 << 4,
        Inactif_depuis_7_jours = 1 << 8,
        Mode_vacances = 1 << 16,
        Blocked = 1 << 32,
        Outlaw = 1 << 64,
        Cible_honorable = 1 << 128,
    }
}