using Newtonsoft.Json;

namespace MapApiCore.Models.LondonAir
{
    public class LocalAuthority
    {
        [JsonProperty("@LocalAuthorityCode")]
        public string LocalAuthorityCode { get; set; }

        [JsonProperty("@LocalAuthorityName")]
    public string LocalAuthorityName { get; set; }

        [JsonProperty("@LaCentreLatitude")]
        public string LaCentreLatitude { get; set; }

        [JsonProperty("@LaCentreLongitude")]
        public string LaCentreLongitude { get; set; }

        [JsonProperty("@LaCentreLatitudeWGS84")]
        public string LaCentreLatitudeWgs84 { get; set; }

        [JsonProperty("@LaCentreLongitudeWGS84")]
        public string LaCentreLongitudeWgs84 { get; set; }

        public Site Site { get; set; }
    }
}
