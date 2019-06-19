using System.Collections.Generic;

namespace MapApiCore.Models
{
    public class Route
    {
        public Route(List<Coordinate> coordinates)
        {
            this.Coordinates = coordinates;
        }

        public decimal Distance { get; set; }

        public string Duration { get; set; }

        public string ModeOfTransport { get; set; }

        public List<Coordinate> Coordinates { get; set; }
    }
}
