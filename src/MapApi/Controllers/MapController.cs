namespace MapApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using MapApiCore.Models.Kml;
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
            return this.Get(1, true, true, new TimeSpan(9, 0, 0));
        }

        // GET api/journey
        [HttpGet("{journeyId}/{showPollution}/{showSchools}/{startTime}")]
        public ActionResult<string> Get(int journeyId, bool showPollution, bool showSchools, TimeSpan startTime)
        {
            IList<EnrichedRoute> fullJourneyOptions = this.ProcessJourney(journeyId, startTime);
            /*
             Kev - List of layers + color
                    List of route + score + color
              Assala - KML file https://stackoverflow.com/questions/952667/how-do-i-generate-a-kml-file-in-asp-net
            */

            var kmlString = this.CreateTestKmlString(showPollution, showSchools);
            return kmlString;
        }

        private string CreateTestKmlString(bool showPollution, bool showSchools)
        {
            var kmlString = System.IO.File.ReadAllText(GetFilePath("Test.kml"));

            if (showPollution)
            {
                var pollutionMarkers = this._pollutionRepo.GetMarkers();
                var pollutionPlacemarks = this.CreatePlacemarks(pollutionMarkers);
                var folder = new Folder { Name = "Pollution", Placemark = pollutionPlacemarks };
                var serializer = new XmlSerializer(typeof(Folder));
                var xout = new StringWriter();

                serializer.Serialize(xout, folder);
                var xml = xout.ToString()
                    .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty)
                    .Replace("<Folder xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<Folder>");
                
                kmlString = kmlString.Replace("{AirQuality}", xml);
            }
            else
            {
                kmlString = kmlString.Replace("{AirQuality}", string.Empty);
            }

            //if (showSchools)
            //{
            //    var schoolMarkers = this._schoolRepo.GetMarkers();
            //    var schoolPlacemarks = this.CreatePlacemarks(schoolMarkers);
            //    var folder = new Folder { Name = "Schools", Placemark = schoolPlacemarks };
            //    var serializer = new XmlSerializer(typeof(Folder));
            //    var xout = new StringWriter();

            //    serializer.Serialize(xout, folder);
            //    var xml = xout.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty);
            //    kmlString = kmlString.Replace("{Schools}", xml);
            //}
            //else
            //{
            //    kmlString = kmlString.Replace("{Schools}", string.Empty);
            //}

            var kml = new XmlDocument();
            kml.LoadXml(kmlString);

            return kml.OuterXml;
        }

        private IList<EnrichedRoute> ProcessJourney(int journeyId, TimeSpan startTime)
        {
            var journeyOptions = _journeyRepo.GetJourney(journeyId);
            var pollutionMarkers = this._pollutionRepo.GetMarkers();
            
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

        private List<Placemark> CreatePlacemarks(List<Marker> markers)
        {
            var placemarks = new List<Placemark>();
            
            foreach (var marker in markers)
            {
                placemarks.Add(new Placemark
                {
                    Name = $"{marker.Description} ({marker.Value})",
                    StyleUrl = "#icon-1769-0F9D58-nodesc",
                    Point = new Point { Coordinates = $"{marker.Coordinate.Longitude},{marker.Coordinate.Latitude}"}
                });    
            }

            return placemarks;
        }
    }
}