using MapApiCore.Models;
using System.Threading.Tasks;

namespace MapApi.Services.Interfaces
{
    public interface IDirectionService
    {
        Task<string> GetAsync(Coordinate source, Coordinate destination);
    }
}
