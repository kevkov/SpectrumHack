namespace MapApiCore.Models.LondonAir
{
    using Newtonsoft.Json;

    public class Species
    {
        [JsonProperty("@SpeciesCode")]
        public string SpeciesCode { get; set; }

        [JsonProperty("@SpeciesDescription")]
        public string SpeciesDescription { get; set; }

        [JsonProperty("@AirQualityIndex")]
        public string AirQualityIndex { get; set; }

        [JsonProperty("@AirQualityBand")]
        public string AirQualityBand { get; set; }

        [JsonProperty("@IndexSource")]
        public string IndexSource { get; set; }
    }
}
