using System.Collections.Generic;

namespace MapApiCore.Models
{
    public class EnrichedRoute
    {
        public IList<Marker> RouteMarkers { get; set; }

        public int PollutionScore { get; set; }
    }
}
