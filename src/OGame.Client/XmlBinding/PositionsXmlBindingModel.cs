using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using OGame.Client.XmlBinding;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("playerData")]
    public class PositionsXmlBindingModel
    {
        [XmlArray("positions")]
        [XmlArrayItem("position", typeof(PositionXmlBinding))]
        public List<PositionXmlBinding> Positions { get; set; }
    }

    public class PositionXmlBinding
    {
        [XmlText()]
        public int Position { get; set; }

        [XmlAttribute("type")]
        public int Type { get; set; }

        [XmlAttribute("score")]
        public int Score { get; set; }

        [XmlAttribute("ships")]
        public int Ships { get; set; }
    }
}

