using System;
using System.Collections.Generic;
using System.Linq;
using OGame.Client.Providers.Web;

namespace OGame.Client.Models
{
    public class Alliance
    {
        internal Alliance(IPlayerProvider playerProvider,IScoreProvider scoreProvider)
        {
            PlayerProvider = playerProvider;
            ScoreProvider = scoreProvider;
        }

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string Tag { get; internal set; }
        public int FounderId { get; internal set; }
        public string Logo { get; set; }
        public string HomePage { get; set; }
        public DateTime FoundDate { get; internal set; }

        public List<int> MemberIds { get; internal set; }

        internal IPlayerProvider PlayerProvider { get; }
        internal IScoreProvider ScoreProvider { get; }

        public AllianceScore Score => ScoreProvider.Get(Id);

        public List<Player> Members
        {
            get
            {
                if (MemberIds == null)
                    return new List<Player>();

                return MemberIds
                    .Select(id => PlayerProvider.Get(id))
                    .ToList();
            }
        }
        public Player Founder => PlayerProvider.Get(FounderId);
        

        public override string ToString()
        {
            return "Name : " + Name ;
        }
    }
}