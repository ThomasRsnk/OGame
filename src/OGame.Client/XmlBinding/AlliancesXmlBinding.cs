using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("alliances")]
    public class AlliancesXmlBinding
    {
        [XmlElement("alliance")]
        public AllianceXmlBinding[] ListAlliances { get; set; }
    }


    public class AllianceXmlBinding
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tag")]
        public string Tag { get; set; }

        [XmlAttribute("founder")]
        public int Founder { get; set; }

        [XmlAttribute("foundDate")]
        public int FoundDate { get; set; }

        [XmlAttribute("logo")]
        public string Logo { get; set; }

        [XmlAttribute("homepage")]
        public string HomePage { get; set; }

        [XmlElement("player")]
        public MemberXmlBinding[] Members { get; set; }
    }

    public class MemberXmlBinding
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
