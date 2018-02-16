using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("highscore")]
    public class HighscoresXmlBinding
    {
        [XmlElement("player")]
        public List<ScoreXmlBinding> Scores { get; set; }

        [XmlElement("alliance")]
        public List<ScoreXmlBinding> Scores_A { get; set; }
    }

    public class ScoreXmlBinding
    {
        [XmlAttribute("position")]
        public int Position { get; set; }
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("score")]
        public int ScoreTotal { get; set; }
    }
}
