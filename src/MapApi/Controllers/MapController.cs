namespace MapApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IPollutionRepository _pollutionRepo;
        private readonly ISchoolRepository _schoolRepo;
        private readonly IJourneyRepository _journeyRepo;
        private readonly IIntersectionService _interactionService;

        public MapController(IPollutionRepository pollutionRepo, ISchoolRepository schoolRepo, IJourneyRepository journeyRepo, IIntersectionService interactionService)
        {
            _pollutionRepo = pollutionRepo;
            _schoolRepo = schoolRepo;
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
            // TODO: Need to add start time as a parameter on the controller
            var startTime = new TimeSpan(9, 0 ,0);
            IList<EnrichedRoute> fullJourneyOptions = ProcessJourney(journeyId, startTime);
            /*
             Kev - List of layers + color
                    List of route + score + color
              Assala - KML file https://stackoverflow.com/questions/952667/how-do-i-generate-a-kml-file-in-asp-net
            */

            var kml = new XmlDocument();
            kml.Load(GetFilePath("Test.kml"));

            return kml.OuterXml;
        }

        private IList<EnrichedRoute> ProcessJourney(int journeyId, TimeSpan startTime)
        {
            var journeyOptions = _journeyRepo.GetJourney(journeyId);
            var pollutionMarkers = _pollutionRepo.GetMarkers();
            var schoolMarkers = _schoolRepo.GetMarkers();

            IList<EnrichedRoute> enrichedRoute = new List<EnrichedRoute>();
            foreach (var journeyOption in journeyOptions.Routes)
            {
                enrichedRoute.Add(new EnrichedRoute()
                {
                    PollutionScore = 100,
                    RouteMarkers = _interactionService.FindMarkersOnRoute(journeyOption.Coordinates, pollutionMarkers, startTime)
                });
            }

            return enrichedRoute;
        }

        private static string GetFilePath(string filename)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"", filename);
            return path;
        }
    }
}