namespace MapApiCore.Models.Kml
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Placemark")]
    public class Placemark
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "styleUrl")]
        public string StyleUrl { get; set; }

        [XmlElement(ElementName = "Point")]
        public Point Point { get; set; }
        
        [XmlElement(ElementName = "LineString")]
        public LineString LineString { get; set; }
    }
}
