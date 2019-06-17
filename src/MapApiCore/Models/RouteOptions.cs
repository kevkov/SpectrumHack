using System;
using System.Collections.Generic;
using System.Text;

namespace MapApiCore.Models
{
    public class RouteOptions
    {
        public MapLocation StartLocation { get; set; }
        public MapLocation EndLocation { get; set; }
        public IList<EnrichedRoute> EnrichedRoute { get; set; }
    }
}
