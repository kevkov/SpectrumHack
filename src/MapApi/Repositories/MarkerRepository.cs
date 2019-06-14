using MapApiCore.Models;
using System.Collections.Generic;

namespace MapApi.Repositories
{
    public interface IMarkerRepository
    {
        List<Marker> GetMarkers();
    }
}
