namespace MapApiCore.Models.Kml
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Folder")]
    public class Folder
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Placemark")]
        public List<Placemark> Placemark { get; set; }
    }

}
