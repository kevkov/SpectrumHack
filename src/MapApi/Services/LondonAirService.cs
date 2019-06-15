namespace MapApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;
    using MapApiCore.Models;
    using MapApiCore.Models.LondonAir;
    
    public class LondonAirService : IPollutionService
    {
        private Uri _baseUri = new Uri("http://api.erg.kcl.ac.uk");
        private const string dailyUri = "/AirQuality/Daily/MonitoringIndex/Latest/SiteCode={0}/JSON";

        private readonly HttpClient _httpClient;
        private Dictionary<string, string> _sites;
        
        public LondonAirService()
        {
            _httpClient = new HttpClient() {BaseAddress = _baseUri };

            _sites = new Dictionary<string, string>
            {
                { "LB4", "Lambeth - Brixton Road" },
                { "CT3", "City of London - Sir John Cass School" },
                { "CT4", "City of London - Beech Street" },
                { "CT6", "City of London - Walbrook Wharf" },
                { "GN6", "Greenwich - John Harrison Way" },
                { "SK6", "Southwark - Elephant and Castle" }
            };
        }

        public async Task<List<Marker>> GetPollutionDataForAllSites()
        {
            var markers = new List<Marker>();

            foreach (var site in this._sites)
            {
                var requestUri = string.Format(dailyUri, site.Key);
                var pollutionItem = await this.ReadFromRemoteApiAsync(requestUri);

                if (pollutionItem != null)
                {
                    var marker = this.ConvertToMarker(pollutionItem);
                    markers.Add(marker);
                }
            }

            return markers;
        }

        private Marker ConvertToMarker(LondonAirPollution pollutionItem)
        {
            var site = pollutionItem.DailyAirQualityIndex.LocalAuthority.Site;
            var longitude = double.Parse(site.Longitude);
            var latitude = double.Parse(site.Latitude);
            var speciesWithAirQualityIndex = site.Species.FirstOrDefault(s => s.AirQualityIndex != null);
            var airQualityIndexValue = speciesWithAirQualityIndex != null ? int.Parse(speciesWithAirQualityIndex.AirQualityIndex) : 0;

            return new Marker(new Coordinate(longitude, latitude), airQualityIndexValue, site.SiteName);
        }

        private async Task<LondonAirPollution> ReadFromRemoteApiAsync(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var londonAirPollutionEntity = await response.Content.ReadAsAsync<LondonAirPollution>();
                return londonAirPollutionEntity;
            }

            return null;
        }
    }
}