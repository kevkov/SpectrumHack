using GoogleMapAPIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace GoogleMapAPIWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Make an HTTP call to the API... convert response to HomeViewModel instead of this
            // var response = HttpClient.blahblah

            var homeViewModel = new HomeViewModel();

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


            return View(homeViewModel);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
