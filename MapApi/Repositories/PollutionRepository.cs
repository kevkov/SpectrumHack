using System.Collections.Generic;
using MapApi.Models;

namespace MapApi.Repositories
{
    public interface IMarkerRepository
    {
        List<PollutionMarker> GetMarkers();
    }

    public class PollutionRepository : IMarkerRepository
    {
        public List<PollutionMarker> GetMarkers()
        {
            return new List<PollutionMarker>
            {
                new PollutionMarker(10, new Coordinate(0.00447, 51.49847)),
                new PollutionMarker(50, new Coordinate(0.00496, 51.49869))
            };
        }
    }
}