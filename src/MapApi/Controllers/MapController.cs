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
            return this.Get(1, new TimeSpan(9, 0, 0), true, true);
        }

        // GET api/journey
        [HttpGet("{journeyId}")]
        public ActionResult<string> Get(int journeyId, TimeSpan startTime, bool showPollution, bool showSchools)
        {
            IList<EnrichedRoute> fullJourneyOptions = this.ProcessJourney(journeyId, startTime);
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