namespace MapApiCore.Models
{
    public struct PointDetails
    {
        public PointDetails(string name, double longitude, double latitude)
        {
            this.Name = name;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude{ get; set; }
    }
}
