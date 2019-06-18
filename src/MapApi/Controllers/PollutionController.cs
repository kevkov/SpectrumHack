namespace MapApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class PollutionController : ControllerBase
    {
        private readonly IPollutionRepository _pollutionRepository;
        private readonly IPollutionService _londonAirService;
        
        public PollutionController(IPollutionRepository pollutionRepository, IPollutionService londonAirService)
        {
            _pollutionRepository = pollutionRepository;
            _londonAirService = londonAirService;
        }

        // GET api/pollution
        [HttpGet]
        public async Task<ActionResult<List<Marker>>> Get()
        {
            var markers = await this._londonAirService.GetPollutionDataForAllSites();
            this._pollutionRepository.CreateMarkers(markers);

            return markers;
        }
    }
}