using GoogleMapAPIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Index2()
        {   
            var homeViewModel = await _mapApiClient.RouteInformationAsync(1, true, true, new TimeSpan(12, 13, 0));

            return View(homeViewModel);
        }

        public IActionResult ViewMap()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
