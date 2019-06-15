namespace MapApi.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MapApiCore.Models;
    
    public interface IPollutionService
    {
        Task<List<Marker>> GetPollutionDataForAllSites();
    }
}
