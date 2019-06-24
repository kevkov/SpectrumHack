using System;
using System.Collections.Generic;
using System.Linq;

namespace MapApi.ViewModels
{
    public class LatLng
    {
        public LatLng(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Polyline
    {
        public Polyline(IEnumerable<LatLng> coordinates)
        {
            Coordinates = coordinates.ToList();
        }

        public List<LatLng> Coordinates { get; set; }

        public int StrokeWidth { get; set; }

        public string StrokeColor { get; set; }
    }

    public class Marker
    {
        public Marker()
        {
            IntersectingRouteIndices = new List<int>();
        }

        public string Title { get; set; }

        public string Image { get; set; }

        public LatLng Coordinates { get; set; }

        public List<int> IntersectingRouteIndices { get; set; }
    }

    public class Map
    {
        public List<Polyline> Lines { get; set; }

        public List<Marker> Markers { get; set; }
    }
}