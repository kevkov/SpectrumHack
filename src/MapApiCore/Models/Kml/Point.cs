namespace MapApiCore.Models.Kml
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Point")]
    public class Point
    {
        [XmlElement(ElementName = "coordinates")]
        public string Coordinates { get; set; }
    }
}
