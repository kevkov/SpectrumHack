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
        
        public async Task<IActionResult> Index()
        {
            var homeViewModel = await _mapApiClient.RouteInformationAsync(1, true, true, new TimeSpan(12, 13, 0));
            
            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
