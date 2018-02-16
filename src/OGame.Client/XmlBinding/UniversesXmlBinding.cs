using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("universes")]
    public class UniversesXmlBinding
    {
        [XmlElement("universe")]
        public List<UniverseXmlBinding> Univers { get; set; }
    }

    public class UniverseXmlBinding
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
