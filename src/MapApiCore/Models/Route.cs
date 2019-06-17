using System.Collections.Generic;

namespace MapApiCore.Models
{
    public class Route
    {
        public Route(List<Coordinate> coordinates)
        {
            this.Coordinates = coordinates;
        }

        public List<Coordinate> Coordinates { get; set; }
    }
}
