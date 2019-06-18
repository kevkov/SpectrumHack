using System;
using System.Collections.Generic;
using System.Text;

namespace MapApiCore.Models
{
    public class RouteOptions
    {
        public PointDetails StartLocation { get; set; }
        public PointDetails EndLocation { get; set; }
        public IList<EnrichedRoute> EnrichedRoute { get; set; }
    }
}
