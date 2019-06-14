using MapApiCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MapApiCore.Repositories;

namespace MapApi.Controllers
{
    using MapApiCore.Interfaces;
    using MapApiDataFeeder.Interfaces;
    using Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IPollutionRepository _pollutionRepo;
        private readonly IJourneyRepository _journeyRepo;
        private readonly IIntersectionService _interactionService;

        public MapController(IPollutionRepository pollutionRepo, IJourneyRepository journeyRepo, IIntersectionService interactionService)
        {
            _pollutionRepo = pollutionRepo;
            _journeyRepo = journeyRepo;
            _interactionService = interactionService;
        }

        // GET api/journey
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Get(1);
        }

        // GET api/journey
        [HttpGet("{journeyId}")]
        public ActionResult<string> Get(int journeyId)
        {
            
            IList<EnrichedRoute> fullJourneyOptions = ProcessJourney(journeyId);
            /*
             Kev - List of layers + color
                    List of route + score + color
              Assala - KML file https://stackoverflow.com/questions/952667/how-do-i-generate-a-kml-file-in-asp-net
            */

            var filePath = Path.Combine(Environment.CurrentDirectory, "Test.kml");
            var kml = new XmlDocument();
            kml.Load(filePath);

            return kml.OuterXml;
        }

        private IList<EnrichedRoute> ProcessJourney(int journeyId)
        {
            var journeyOptions = _journeyRepo.GetRoutesForJourney(journeyId);
            var pollutionMarkers = _pollutionRepo.GetMarkers();

            IList<EnrichedRoute> enrichedRoute = new List<EnrichedRoute>();
            foreach (var journeyOption in journeyOptions)
            {
                enrichedRoute.Add(new EnrichedRoute()
                {
                    PollutionScore = 100,
                    RouteMarkers = _interactionService.FindMarkersOnRoute(journeyOption.Coordinates, pollutionMarkers)
                });
            }

            return enrichedRoute;
        }
    }
}