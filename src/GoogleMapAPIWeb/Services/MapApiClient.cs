using GoogleMapAPIWeb.Models;
using System;
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
            var endPoint = $"1/{showPollution}/{showSchools}/{startTime}";

            var response = await _httpClient.GetAsync(endPoint);

            HomeViewModel homeViewModel = default(HomeViewModel);

            if (response.IsSuccessStatusCode)
            {
                var routeInfo = response.Content.ReadAsStringAsync();
            }

            homeViewModel = StubData();

            return homeViewModel;
        }

        private HomeViewModel StubData()
        {
            HomeViewModel homeViewModel = new HomeViewModel();

            homeViewModel.RouteInfos.Add(new RouteInfo
            {
                Id = 1,
                AveragePollutionPoint = 5,
                ColorInHex = "#ff0000",
                SchoolCount = 9,
                TravellTime = new TimeSpan(2, 33, 0)
            });

            homeViewModel.RouteInfos.Add(new RouteInfo
            {
                Id = 2,
                AveragePollutionPoint = 3,
                ColorInHex = "#00ff00",
                SchoolCount = 6,
                TravellTime = new TimeSpan(2, 33, 0)
            });

            homeViewModel.RouteInfos.Add(new RouteInfo
            {
                Id = 3,
                AveragePollutionPoint = 1,
                ColorInHex = "#0000ff",
                SchoolCount = 3,
                TravellTime = new TimeSpan(2, 33, 0)
            });

            return homeViewModel;
        }
    }
}
