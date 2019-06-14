using System.Collections.Generic;
using MapApi.Models;

namespace MapApi.Repositories
{
    public interface IMarkerRepository
    {
        List<Marker> GetMarkers();
    }
}
