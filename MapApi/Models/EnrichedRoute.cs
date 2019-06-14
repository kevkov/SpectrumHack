using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapApi.Models
{
    public class EnrichedRoute
    {
        public Route Route { get; set; }
        public int PollutionScore { get; set; }
    }
}
