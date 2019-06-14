namespace MapApiCore.Models
{
    public struct Coordinate
    {
        public Coordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }
    }
}
