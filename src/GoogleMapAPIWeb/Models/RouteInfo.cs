using System;

namespace GoogleMapAPIWeb.Models
{
    public class RouteInfo
    {
        public int Id { get; set; }

        public string ColorInHex { get; set; }

        public int AveragePollutionPoint { get; set; }

        public TimeSpan TravellTime { get; set; }

        public int SchoolCount { get; set; }
    }
}
