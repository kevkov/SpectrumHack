using MapApiCore.Models;
using System.Collections.Generic;

namespace MapApi.Repositories
{
    public class PollutionRepository : IMarkerRepository
    {
        public List<Marker> GetMarkers()
        {
            return new List<Marker>
            {
                new Marker(new Coordinate(0.00447, 51.49847), 10, "Low"),
                new Marker(new Coordinate(0.00496, 51.49869), 50, "Med")
            };
        }
    }
}