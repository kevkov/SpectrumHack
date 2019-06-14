
namespace MapApi.Models
{
    public class PollutionMarker
    {
        public PollutionMarker(int level, Coordinate coordinate)
        {
            this.Level = level;
            this.Coordinate = coordinate;
        }

        public Coordinate Coordinate { get; set; }

        public int Level { get; set; }
    }
}
