namespace MapApiCore.Models.Kml
{
    using System.Drawing;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Placemark")]
    public class Placemark
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "styleUrl")]
        public string StyleUrl { get; set; }

        [XmlElement(ElementName = "Point")]
        public Point Point { get; set; }
    }
}
