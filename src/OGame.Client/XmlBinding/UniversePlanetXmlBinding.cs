using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("universe")]
    public class UniversePlanetXmlBinding
    {
        [XmlElement("planet")]
        public List<PlanetXmlBinding> Planets { get; set; }
    }
}
