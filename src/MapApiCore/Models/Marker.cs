
namespace MapApiCore.Models
{
    using System;

    public class Marker
    {
        public Marker(Coordinate coordinate, int value, string description, TimeSpan startTime = default(TimeSpan), TimeSpan endTime = default(TimeSpan))
        {
            this.Coordinate = coordinate;
            this.Value = value;
            this.Description = description;
            this.StartTime = startTime;
            this.EndTime = endTime;

            if (startTime == default(TimeSpan) && endTime == default(TimeSpan))
            {
                this.EndTime = new TimeSpan(23, 59, 59);
            }
        }
        
        public Coordinate Coordinate { get; set; }

        public int Value { get; set; }

        public string Description { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
