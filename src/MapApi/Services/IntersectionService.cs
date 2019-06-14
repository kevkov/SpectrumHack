using MapApiCore.Models;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;

namespace MapApi.Services
{
    using Interfaces;

    public class IntersectionService : IIntersectionService
    {
        private const double RangeInMeters = 1000;

        public List<Marker> FindMarkersOnRoute(List<Coordinate> route, List<Marker> markers)
        {
            var matchedMarkers = new List<Marker>();

            foreach (var marker in markers)
            {
                if (route.Any(r => CalculateDistance(r, marker.Coordinate) <= RangeInMeters))
                {
                    matchedMarkers.Add(marker);
                }
            }

            return matchedMarkers;
        }

        private double CalculateDistance(Coordinate first, Coordinate second)
        {
            var firstGeoCoordinate = new GeoCoordinate(first.Latitude, first.Longitude);
            var secondGeoCoordinate = new GeoCoordinate(second.Latitude, second.Longitude);

            return firstGeoCoordinate.GetDistanceTo(secondGeoCoordinate);
        }
    }
}
