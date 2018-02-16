using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OGame.Client.Providers;
using OGame.Client.Providers.Web;

namespace OGame.Client.Models
{
    public class Score 
    {
        public int Id { get; set; }
        public int Points { get; internal set; }
        public int Rank { get; internal set; }
    }

    public class PlayerScore : Score
    {
        internal IPlayerProvider PlayerProvider { get; }

        internal PlayerScore(IPlayerProvider playerProvider)
        {
            PlayerProvider = playerProvider;
        }

        public Player Player => PlayerProvider.Get(Id);

    }

    public class AllianceScore : Score
    {
        internal IAllianceProvider AllianceProvider { get; }

        internal AllianceScore(IAllianceProvider allianceProvider)
        {
            AllianceProvider = allianceProvider;
        }

        public Alliance Alliance => AllianceProvider.Get(Id);
    }

    public class Position
    {
        public int Type { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public TypeClassement TypeC { get; set; }
        public int Ships { get; set; }
        public enum TypeClassement
        {
            Général=0,
            Économie,
            Recherche,
            Militaire,
            Militaires_perdus,
            Militaires_construits,
            Militaires_detruits,
            Honorifique
        }
    }





}
