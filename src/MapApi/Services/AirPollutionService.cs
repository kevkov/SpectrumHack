namespace MapApiDataFeeder.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using MapApiDataFeeder.Interfaces;

    public class AirPollutionService : IAirPollutionService
    {
        //private const string apiKey = "zZTKfHCg9CnMEcKvk"; // Mohammad's Key
        private const string apiKey = "ZhvvjFjWNBq7vWfpC"; // Tim's Key

        private readonly HttpClient _httpClient;

        private readonly IPollutionRepository _pollutionRepository;
        
        public AirPollutionService(IPollutionRepository pollutionRepository)
        {
            _pollutionRepository = pollutionRepository;
            _httpClient = new HttpClient { BaseAddress = new Uri("http://api.airvisual.com") };
        }

        public async Task AirPollutionByCoordinateAsync(Coordinate coordinate)
        {
            var apiPath = $"/v2/nearest_city?lat={coordinate.Latitude}&lon={coordinate.Longitude}&key={apiKey}";

            var marker = await this.ReadFromRemoteApiAsync(apiPath);

            if (marker != null)
            {
                _pollutionRepository.InsertMarker(marker);
            }

        }

        private async Task<Marker> ReadFromRemoteApiAsync(string apiPath)
        {
            Marker marker = null;

            var response = await _httpClient.GetAsync(apiPath);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<AirVisualEntity>();

                if (content?.data != null && content.status != null && content.status.Equals("success"))
                {
                    var responseData = content.data;
                    var locationCoordinates = responseData.location?.coordinates;
                    var pollutionPoints = responseData.current?.pollution;
                    var city = responseData.city;

                    if (pollutionPoints != null && locationCoordinates != null && locationCoordinates.Length == 2)
                    {
                        var coordinate = new Coordinate(locationCoordinates[0], locationCoordinates[1]);
                        marker = new Marker(coordinate, pollutionPoints.aqius, city);
                    }
                }
            }

            return marker;
        }
    }
}
