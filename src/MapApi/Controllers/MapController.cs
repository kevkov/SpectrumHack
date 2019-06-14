using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MapApi.Models;
using MapApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private IMarkerRepository _markerRepo;
        private IJourneyRepository _journeyRepo;
        private IIntersectionService _interactionService;
        
        public MapController(IMarkerRepository markerRepo, IJourneyRepository journeyRepo, IIntersectionService interactionService)
        {
            _markerRepo = markerRepo;
            _journeyRepo = journeyRepo;
            _interactionService = interactionService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get(int journeyId)
        {

            IList<EnrichedRoute> fullJourneyOptions = processJourney(journeyId);
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

        private IList<EnrichedRoute> processJourney(int journeyId)
        {
            var journeyOptions = _journeyRepo.GetRoutesForJourney(journeyId);
            var pollutionMarkers = _markerRepo.GetMarkers();

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
