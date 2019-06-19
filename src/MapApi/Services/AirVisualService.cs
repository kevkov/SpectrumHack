namespace MapApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;
    using MapApiCore.Models;
    
    public class AirVisualService : IPollutionService
    {
        private const string ApiKey = "ZhvvjFjWNBq7vWfpC"; // Tim's Key

        private readonly HttpClient _httpClient;
        private readonly List<Coordinate> _coordinates;
        
        public AirVisualService()
        {
            this._httpClient = new HttpClient { BaseAddress = new Uri("http://api.airvisual.com") };

            _coordinates = new List<Coordinate>
            {
                new Coordinate(0.00447, 51.49847)
            };
        }

        public async Task<List<Marker>> GetPollutionDataForAllSites()
        {
            var markers = new List<Marker>();

            foreach (var coordinate in this._coordinates)
            {
                var requestUri = $"/v2/nearest_city?lat={coordinate.Latitude}&lon={coordinate.Longitude}&key={ApiKey}";

                var airVisualEntity = await this.ReadFromRemoteApiAsync(requestUri);

                if (airVisualEntity != null)
                {
                    var marker = ConvertToMarker(airVisualEntity);
                    markers.Add(marker);
                }
            }

            return markers;
        }
        
        private async Task<AirVisualEntity> ReadFromRemoteApiAsync(string requestUri)
        {
            var response = await this._httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var airVisualEntity = await response.Content.ReadAsAsync<AirVisualEntity>();

                if (airVisualEntity?.data != null && airVisualEntity.status != null && airVisualEntity.status.Equals("success"))
                {
                    return airVisualEntity;
                }
            }

            return null;
        }

        private static Marker ConvertToMarker(AirVisualEntity airVisualEntity)
        {
            var responseData = airVisualEntity.data;
            var locationCoordinates = responseData.location?.coordinates;
            var pollutionPoints = responseData.current?.pollution;
            var city = responseData.city;

            if (pollutionPoints != null && locationCoordinates != null && locationCoordinates.Length == 2)
            {
                var coordinate = new Coordinate(locationCoordinates[0], locationCoordinates[1]);
                return new Marker(coordinate, pollutionPoints.aqius, city);
            }

            return null;
        }
    }
}
