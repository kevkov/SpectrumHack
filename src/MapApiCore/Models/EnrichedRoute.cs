using System.Collections.Generic;

namespace MapApiCore.Models
{
    public class EnrichedRoute
    {
        public IList<Marker> RouteMarkers { get; set; }
        public IList<Marker> PollutionMarkers { get; set; }
        public IList<Marker> SchoolMarkers { get; set; }

        public int GreenScore { get; set; }

        public decimal Cost { get; set; }

        public string Colour { get; set; }

        public string Label { get; set; }

        public string Distance { get; set; }

        public string Duration { get; set; }

    }
}
