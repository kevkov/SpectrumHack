using System.Drawing;
using System.Text;

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
            RouteOptions fullJourneyOptions = this.ProcessJourney(journeyId, startTime);

            var kmlString = this.CreateTestKmlString(fullJourneyOptions, showPollution, showSchools);
            return kmlString;
        }

        private string CreateTestKmlString(RouteOptions routeOptions, bool showPollution, bool showSchools)
        {
            var kmlString = System.IO.File.ReadAllText(GetFilePath("Test.kml"));
            
            var styles = GetStyles(routeOptions);
            kmlString = kmlString.Replace("{Styles}", styles);

            var routes = GetRoutes(routeOptions);
            kmlString = kmlString.Replace("{Routes}", routes);
            kmlString = kmlString.Replace("<ArrayOfFolder xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", string.Empty);
            kmlString = kmlString.Replace("</ArrayOfFolder>", string.Empty);

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

            if (showSchools)
            {
                var schoolMarkers = this._schoolRepo.GetMarkers();
                var schoolPlacemarks = this.CreatePlacemarks(schoolMarkers);
                var folder = new Folder { Name = "Schools", Placemark = schoolPlacemarks };
                var serializer = new XmlSerializer(typeof(Folder));
                var xout = new StringWriter();

                serializer.Serialize(xout, folder);
                var xml = xout.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty);
                kmlString = kmlString.Replace("{Schools}", xml);
            }
            else
            {
                kmlString = kmlString.Replace("{Schools}", string.Empty);
            }

            var kml = new XmlDocument();
            kml.LoadXml(kmlString);

            return kml.OuterXml;
        }

        private string GetStyles(RouteOptions routeOptions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var route in routeOptions.EnrichedRoute)
            {
                sb.AppendLine(String.Format(styleString, $"#line-{route.GreenScore}-{route.Cost}-{route.Colour}", route.Colour));
            }

            return sb.ToString();
        }

        string styleString =
            "<Style id=\"{0}\">"+
            "<IconStyle> " +
            "<color>{1}</color> " +
            "<scale>1</scale> "+
            "<Icon> "+
            "<href>http://www.gstatic.com/mapspro/images/stock/503-wht-blank_maps.png</href>"+
            "</Icon>"+
            "</IconStyle>"+
            "<BalloonStyle>"+
            "<text><![CDATA[<h3>$[name]</h3>]]></text>"+
            "</BalloonStyle>"+
            "</Style>";

        private string GetRoutes(RouteOptions routeOptions)
        {
            var folder = GetRouteFolders(routeOptions);
            var serializer = new XmlSerializer(typeof(List<Folder>));
            var xout = new StringWriter();

            serializer.Serialize(xout, folder);
            var xml = xout.ToString()
                .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty)
                .Replace("<Folder xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<Folder>");

            return xml;
        }

        private List<Folder> GetRouteFolders(RouteOptions routeOptions)
        {
            var folders = new List<Folder>();

            foreach (var route in routeOptions.EnrichedRoute)
            {
                folders.Add(
                    new Folder()
                    {
                        Name = $"From {routeOptions.StartLocation.Name} to {routeOptions.EndLocation.Name}",
                        Placemark = new List<Placemark>()
                        {
                            // route placemark
                            new Placemark()
                            {
                                StyleUrl = $"#line-{route.GreenScore}-{route.Cost}-{route.Colour}",
                                LineString = new LineString()
                                {
                                    Tessellate = 1,
                                    Coordinates = string.Join(',', route.RouteMarkers.ToList().Select(x => $"{x.Coordinate.Longitude},{x.Coordinate.Latitude},0{Environment.NewLine}"))
                                }
                            },
                            // start placemark
                            new Placemark()
                            {
                                Name = routeOptions.StartLocation.Name,
                                StyleUrl = "#icon-1899-DB4436-nodesc-normal",
                                Point = new Point()
                                {
                                    Coordinates = $"{routeOptions.StartLocation.Longitude},{routeOptions.StartLocation.Latitude},0"
                                }
                            },
                            // end placemark
                            new Placemark()
                            {
                                Name = routeOptions.EndLocation.Name,
                                StyleUrl = "#icon-1899-DB4436-nodesc-normal",
                                Point = new Point()
                                {
                                    Coordinates = $"{routeOptions.EndLocation.Longitude},{routeOptions.EndLocation.Latitude},0"
                                }
                            }
                        }
                    });
            }

            return folders;
        }

        private RouteOptions ProcessJourney(int journeyId, TimeSpan startTime)
        {
            var journeyOptions = _journeyRepo.GetJourney(journeyId);
            var pollutionMarkers = this._pollutionRepo.GetMarkers();
            var schoolMarkers = this._schoolRepo.GetMarkers();


            IList<EnrichedRoute> enrichedRoute = new List<EnrichedRoute>();
            foreach (var journeyOption in journeyOptions.Routes)
            {
                var er = new EnrichedRoute()
                {
                    RouteMarkers = journeyOption.Coordinates.Select(x => new Marker(new Coordinate(x.Longitude, x.Latitude), 0, string.Empty)).ToList(),
                    PollutionMarkers = _interactionService.FindMarkersOnRoute(journeyOption.Coordinates, pollutionMarkers, startTime),
                    SchoolMarkers = _interactionService.FindMarkersOnRoute(journeyOption.Coordinates, schoolMarkers, startTime)
                };

                er.GreenScore = Math.Min(100, 100  
                                              - (er.PollutionMarkers.Count * 10)
                                              - (er.SchoolMarkers.Count * 20)
                                              );
                er.Cost = Math.Min(30,
                    20 + ((100-er.GreenScore)/10)
                    );

                er.Colour = GetBlendedColor((int.Parse(er.Cost.ToString())-20)*10).Name;

                // Layman terms - £20 + £1 for pollution mark, + £2 for school mark upto £30

                // distance or time spent to add?
                // 7.5 miles (1.5x east-west distance) = £30
                // points - starts at 100 -20 for school, -10 for within 200m of pollution points
                // £20 base + (100-points/10)

                enrichedRoute.Add(er);
            }

            return new RouteOptions()
            {
                StartLocation = journeyOptions.Start,
                EndLocation = journeyOptions.End,
                EnrichedRoute = enrichedRoute
            };
        }

        private Color GetBlendedColor(int percentage)
        {
            if (percentage < 50)
                return Interpolate(Color.Red, Color.Yellow, percentage / 50.0);
            return Interpolate(Color.Yellow, Color.Lime, (percentage - 50) / 50.0);
        }

        private Color Interpolate(Color color1, Color color2, double fraction)
        {
            double r = Interpolate(color1.R, color2.R, fraction);
            double g = Interpolate(color1.G, color2.G, fraction);
            double b = Interpolate(color1.B, color2.B, fraction);
            return Color.FromArgb((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
        }

        private double Interpolate(double d1, double d2, double fraction)
        {
            return d1 + (d2 - d1) * fraction;
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
                    StyleUrl = "#icon-1769-0F9D58-nodesc-normal",
                    Point = new Point { Coordinates = $"{marker.Coordinate.Longitude},{marker.Coordinate.Latitude}"}
                });    
            }

            return placemarks;
        }
    }
}