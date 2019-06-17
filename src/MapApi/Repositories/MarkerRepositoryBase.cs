namespace MapApi.Repositories
{
    using System.Collections.Generic;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;

    public abstract class MarkerRepositoryBase : RepositoryBase, IMarkerRepository
    {
        private readonly List<Marker> _markers;

        public MarkerRepositoryBase()
        {
            this._markers = ReadData<Marker>(this.DataFileName);
        }

        protected abstract string DataFileName { get; }

        public List<Marker> GetMarkers()
        {
            return this._markers;
        }

        public void CreateMarkers(List<Marker> markers)
        {
            this._markers.Clear();
            this._markers.AddRange(markers);

            WriteData(this.DataFileName, markers);
        }
    }
}
