using System.Collections.Generic;
using MapApiCore.Models;

namespace MapApiCore.Repositories
{
    using System.Linq;
    using Interfaces;
    using Newtonsoft.Json;

    public class PollutionRepository : RepositoryBase, IPollutionRepository
    {
        private const string fileName = "Pollution.json";

        private readonly List<Marker> _markers;

        public PollutionRepository()
        {
            _markers = ReadData<Marker>(fileName);
        }

        public List<Marker> GetMarkers()
        {
            return _markers;
        }

        public void InsertMarker(Marker marker)
        {
            var markerExists = this._markers.Any(m => m.Coordinate.Longitude == marker.Coordinate.Longitude && m.Coordinate.Latitude == marker.Coordinate.Latitude);

            if (markerExists == false)
            {
                _markers.Add(marker);
                WriteData(fileName, this._markers);
            }
            
        }
    }
}