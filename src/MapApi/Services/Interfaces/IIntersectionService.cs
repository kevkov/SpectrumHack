using System.Collections.Generic;

namespace MapApi.Services.Interfaces
{
    using MapApiCore.Models;

    public interface IIntersectionService
    {
        List<Marker> FindMarkersOnRoute(List<Coordinate> route, List<Marker> markers);
    }
}
