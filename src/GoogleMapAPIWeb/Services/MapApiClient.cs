using GoogleMapAPIWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleMapAPIWeb.Services
{
    public class MapApiClient : IMapApiClient
    {
        private readonly HttpClient _httpClient;

        public MapApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HomeViewModel> RouteInformationAsync(int journeyId, bool showPollution, bool showSchools, TimeSpan startTime)
        {
            var endPoint = $"routes/{journeyId}/{showPollution}/{showSchools}/{startTime}";

            var response = await _httpClient.GetAsync(endPoint);

            HomeViewModel homeViewModel = new HomeViewModel();

            if (response.IsSuccessStatusCode)
            {
                homeViewModel.RouteInfos = await response.Content.ReadAsAsync<List<RouteInfo>>();
            }

            return homeViewModel;
        }
    }
}
