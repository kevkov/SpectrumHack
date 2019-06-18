namespace MapApiCore.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IMarkerRepository
    {
        List<Marker> GetMarkers();

        void InsertMarker(Marker marker);
    }
}
