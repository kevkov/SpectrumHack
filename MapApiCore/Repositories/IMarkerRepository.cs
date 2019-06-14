using System.Collections.Generic;
using MapApiCore.Models;

namespace MapApiCore.Repositories
{
    public interface IMarkerRepository
    {
        List<Marker> GetMarkers();
    }
}
