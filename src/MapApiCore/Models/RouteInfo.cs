﻿using System;

namespace GoogleMapAPIWeb.Models
{
    public class RouteInfo
    {
        public string ColorInHex { get; set; }
        public string RouteLabel { get; set; }
        public int PollutionPoint { get; set; }
        public string Duration { get; set; }
        public decimal TravelCost { get; set; }
        public int SchoolCount { get; set; }
        public string Distance { get; set; }
    }
}
