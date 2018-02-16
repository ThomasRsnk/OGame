using System.Collections.Generic;
using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("playerData")]
    public class PlanetsXmlBinding
    {
        [XmlArray("planets")]
        [XmlArrayItem("planet",typeof(PlanetXmlBinding))]
        public List<PlanetXmlBinding> Planets { get; set; }
    }

    public class PlanetXmlBinding
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("coords")]
        public string Coords { get; set; }

        [XmlAttribute("player")]
        public int PlayerId { get; set; }

        [XmlElement("moon")]
        public MoonXmlBinding Lune { get; set; }

    }

    public class MoonXmlBinding
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }
    }
}
