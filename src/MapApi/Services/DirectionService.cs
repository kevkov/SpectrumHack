using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MapApi.Services.Interfaces;
using MapApiCore.Models;

namespace MapApi.Services
{
    public class DirectionService: IDirectionService
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
            string jsonResponse = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
            }

            return jsonResponse;
        }
    }
}
