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
    }

    public class Map
    {
        public List<Polyline> Lines { get; set; }
    }
}