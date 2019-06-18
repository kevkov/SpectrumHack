using MapApiCore.Models;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;

namespace MapApi.Services
{
    using System;
    using Interfaces;

    public class IntersectionService : IIntersectionService
    {
        private const double RangeInMeters = 1000;

        public List<Marker> FindMarkersOnRoute(List<Coordinate> route, List<Marker> markers, TimeSpan startTime)
        {
            var matchedMarkers = new List<Marker>();

            foreach (var marker in markers)
            {
                // Point must be within range and valid for the start time of the journey
                if (route.Any(r => CalculateDistance(r, marker.Coordinate) <= RangeInMeters && 
                                   startTime >= marker.StartTime && 
                                   startTime <= marker.EndTime))
                {
                    matchedMarkers.Add(marker);
                }
            }

            return matchedMarkers;
        }

        private static double CalculateDistance(Coordinate first, Coordinate second)
        {
            var firstGeoCoordinate = new GeoCoordinate(first.Latitude, first.Longitude);
            var secondGeoCoordinate = new GeoCoordinate(second.Latitude, second.Longitude);

            return firstGeoCoordinate.GetDistanceTo(secondGeoCoordinate);
        }
    }
}
