namespace MapApiCore.Models.LondonAir
{
    using System.Collections.Generic;
    using Converters;
    using Newtonsoft.Json;

    public class Site
    {
        [JsonProperty("@BulletinDate")]
        public string BulletinDate { get; set; }

        [JsonProperty("@SiteCode")]
        public string SiteCode { get; set; }

        [JsonProperty("@SiteName")]
        public string SiteName { get; set; }

        [JsonProperty("@SiteType")]
        public string SiteType { get; set; }

        [JsonProperty("@Latitude")]
        public string Latitude { get; set; }

        [JsonProperty("@Longitude")]
        public string Longitude { get; set; }

        [JsonProperty("@LatitudeWGS84")]
        public string @LatitudeWgs84 { get; set; }

        [JsonProperty("@LongitudeWGS84")]
        public string LongitudeWgs84 { get; set; }

        [JsonConverter(typeof(SingleValueArrayConverter<Species>))]
        public List<Species> Species { get; set; }
    }
}
