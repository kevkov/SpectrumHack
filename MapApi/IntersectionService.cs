using System.Collections.Generic;
using System.Linq;
using MapApi.Models;

namespace MapApi
{
    public interface IIntersectionService
    {
        List<Marker> FindMarkersOnRoute(List<Coordinate> route, List<Marker> markers);
    }

    public class IntersectionService : IIntersectionService
    {
        public List<Marker> FindMarkersOnRoute(List<Coordinate> route, List<Marker> markers)
        {
            return markers.Where(m => route.Contains(m.Coordinate)).ToList();
        }
    }
}
