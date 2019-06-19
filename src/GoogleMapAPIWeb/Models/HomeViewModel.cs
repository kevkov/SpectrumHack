using System.Collections.Generic;

namespace GoogleMapAPIWeb.Models
{
    public class HomeViewModel
    {

        public HomeViewModel()
        {
            RouteInfos = new List<RouteInfo>();
        }

        public List<RouteInfo> RouteInfos { get; set; }
    }

}
