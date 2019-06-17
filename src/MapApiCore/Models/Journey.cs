using System.Collections.Generic;

namespace MapApiCore.Models
{
    public class Journey
    {
        public int JourneyId { get; set; }

        public List<Route> Routes { get; set; }

        public PointDetails Start { get; set; }

        public PointDetails End { get; set; }
    }
}
