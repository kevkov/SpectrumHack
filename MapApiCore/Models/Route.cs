using System.Collections.Generic;

namespace MapApiCore.Models
{
    public class Route
    {
        public Route(int journeyId, List<Coordinate> coordinates)
        {
            this.JourneyId = journeyId;
            this.Coordinates = coordinates;
        }

        public int JourneyId { get; set; }

        public List<Coordinate> Coordinates { get; set; }
    }
}
