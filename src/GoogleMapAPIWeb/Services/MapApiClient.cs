using GoogleMapAPIWeb.Models;
using System;
using System.Net.Http;

namespace GoogleMapAPIWeb.Services
{
    public class MapApiClient
    {
        private readonly HttpClient _httpClient;

        public MapApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HomeViewModel RouteInformation(int journeyId, bool showPollution, bool showSchools, TimeSpan startTime)
        {
            throw new NotImplementedException();
        }
    }
}
