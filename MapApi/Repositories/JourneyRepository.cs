using System.Collections.Generic;
using MapApi.Models;

namespace MapApi.Repositories
{
    public interface IJourneyRepository
    {
        List<Route> GetRoutesForJourney(int journeyId);
    }

    public class JourneyRepository : IJourneyRepository
    {
        public List<Route> GetRoutesForJourney(int journeyId)
        {
            var route1 = new Route(
                journeyId,
                new List<Coordinate>
                {
                    new Coordinate(0.00447, 51.49847),
                    new Coordinate(0.00496, 51.49869)
                });

            var route2 = new Route(
                journeyId,
                new List<Coordinate>
                {
                    new Coordinate(0.00447, 51.49847),
                    new Coordinate(0.00496, 51.498690)
                });

            var route3 = new Route(
                journeyId,
                new List<Coordinate>
                {
                    new Coordinate(0.00447, 51.49847),
                    new Coordinate(0.00496, 51.49869)
                });

            return new List<Route> {route1, route2, route3};
        }
    }
}
