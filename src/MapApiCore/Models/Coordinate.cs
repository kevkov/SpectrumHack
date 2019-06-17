namespace MapApiCore.Models
{
    public struct Coordinate
    {
        public Coordinate(double longitude, double latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }
    }
}
