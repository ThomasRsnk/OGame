using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("players")]
    public class PlayersXmlBinding
    {
        [XmlElement("player")]
        public PlayerXmlBinding[] ListPlayers { get; set; }
    }

    public class PlayerXmlBinding
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("status")]
        public string Status { get; set; }

        [XmlAttribute("alliance")]
        public string Alliance { get; set; }

        public int? AllianceId => string.IsNullOrWhiteSpace(Alliance) ? (int?)null : int.Parse(Alliance);
    }
}
