using GoogleMapAPIWeb.Models;
using System;
using System.Threading.Tasks;

namespace GoogleMapAPIWeb.Services
{
    public interface IMapApiClient
    {
        Task<HomeViewModel> RouteInformationAsync(int journeyId, bool showPollution, bool showSchools, TimeSpan startTime);
    }
}
