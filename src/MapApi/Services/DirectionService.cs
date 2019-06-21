using MapApi.Services.Interfaces;
using MapApiCore.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MapApi.Services
{
    public class DirectionService : IDirectionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public DirectionService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }
        public async Task<string> GetAsync(Coordinate source, Coordinate destination)
        {
            string endPoint = $"directions/xml?origin={source.ToString()}&destination={destination.ToString()}&alternatives=true&key={_apiKey}";

            var response = await _httpClient.GetAsync(endPoint);
            string xmlResponse = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                xmlResponse = await response.Content.ReadAsStringAsync();
            }

            return xmlResponse;
        }
    }
}
