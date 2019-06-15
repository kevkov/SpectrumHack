using System.Collections.Generic;
using MapApiCore.Models;

namespace MapApiCore.Repositories
{
    using Interfaces;
    
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

        public void CreateMarkers(List<Marker> markers)
        {
            _markers.Clear();
            _markers.AddRange(markers);

            WriteData(fileName, markers);
        }
    }
}