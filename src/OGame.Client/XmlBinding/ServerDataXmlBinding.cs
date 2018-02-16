using System.Xml.Serialization;

namespace OGame.Client.XmlBinding
{
    [XmlRoot("serverData")]
    public class ServerDataXmlBinding
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("number")]
        public int Id { get; set; }
    }
}