namespace MapApiCore.Models.LondonAir
{
    using Newtonsoft.Json;

    public class DailyAirQualityIndex
    {
        [JsonProperty("@MonitoringIndexDate")]
        public string MonitoringIndexDate { get; set; }

        [JsonProperty("@TimeToLive")]
        public string TimeToLive { get; set; }

        public LocalAuthority LocalAuthority { get; set; }
    }
}
