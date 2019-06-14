using MapApiCore.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MapApiCore.Repositories
{
    public class PollutionRepository : IPollutionRepository
    {
        private readonly HttpClient _httpClient;

        public PollutionRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Marker> MarkersAsync(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"Path '{path}' is invalid");
            }

            Marker marker = default(Marker);

            var response = await _httpClient.GetAsync(path);

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