using System.Collections.Generic;
using MapApiCore.Models;

namespace MapApiCore.Repositories
{
    using Interfaces;

    public class PollutionRepository : IPollutionRepository
    {
        public List<Marker> GetMarkers()
        {
            return new List<Marker>
            {
                new Marker(new Coordinate(0.00447, 51.49847), 10, "Low"),
                new Marker(new Coordinate(0.00496, 51.49869), 50, "Med")
            };
        }

        public void InsertMarker(Marker marker)
        {

        }
    }
}