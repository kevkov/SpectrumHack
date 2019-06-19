using GoogleMapAPIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GoogleMapAPIWeb.Services;

namespace GoogleMapAPIWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapApiClient _mapApiClient;

        public HomeController(IMapApiClient mapApiClient)
        {
            _mapApiClient = mapApiClient;
        }
        
        public async Task<IActionResult> Index()
        {
            //var homeViewModel = await _mapApiClient.RouteInformationAsync(1, true, true, new TimeSpan(12, 13, 0));
            var homeViewModel = new HomeViewModel();
            homeViewModel.RouteInfos = new List<RouteInfo>();
            homeViewModel.RouteInfos.Add(new RouteInfo{ ColorInHex = "#ff0000", PollutionPoint = 4, RouteLabel = "Route A", SchoolCount = 4});
            homeViewModel.RouteInfos.Add(new RouteInfo{ ColorInHex = "#00ff00", PollutionPoint = 4, RouteLabel = "Route B", SchoolCount = 4});
            homeViewModel.RouteInfos.Add(new RouteInfo{ ColorInHex = "#0000ff", PollutionPoint = 4, RouteLabel = "Route C", SchoolCount = 4});

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
