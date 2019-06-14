using MapApiCore.Models;
using System.Threading.Tasks;

namespace MapApiDataFeeder.Interfaces
{
    public interface IAirPollutionService
    {
        Task AirPollutionByCoordinateAsync(Coordinate coordinate);
    }
}
