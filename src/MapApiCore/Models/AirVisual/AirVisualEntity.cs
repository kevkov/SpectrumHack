using System;
using System.Collections.Generic;
using System.Text;

namespace MapApiCore.Models
{
    public class AirVisualEntity
    {
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public Location location { get; set; }
        public Current current { get; set; }
    }

    public class Location
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Current
    {
        public Weather weather { get; set; }
        public Pollution pollution { get; set; }
    }

    public class Weather
    {
        public DateTime ts { get; set; }
        public int tp { get; set; }
        public int pr { get; set; }
        public int hu { get; set; }
        public float ws { get; set; }
        public int wd { get; set; }
        public string ic { get; set; }
    }

    public class Pollution
    {
        public DateTime ts { get; set; }
        public int aqius { get; set; }
        public string mainus { get; set; }
        public int aqicn { get; set; }
        public string maincn { get; set; }
    }

}
