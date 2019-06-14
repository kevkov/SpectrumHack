using MapApiCore.Models;
using System.Threading.Tasks;

namespace MapApiCore.Repositories
{
    public interface IPollutionRepository
    {
        Task<Marker> MarkersAsync(string path);
    }
}
