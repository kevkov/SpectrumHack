using System.Xml.Serialization;

namespace MapApiCore.Models.Kml
{
    [XmlRoot(ElementName = "LineString")]
    public class LineString
    {
        [XmlElement(ElementName = "tessellate")]
        public int Tessellate { get; set; }

        [XmlElement(ElementName = "coordinates")]
        public string Coordinates { get; set; }
    }
}