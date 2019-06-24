namespace MapApi.Controllers
{
    using MapApi.ViewModels;
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using MapApiCore.Models.Kml;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private Journey journey = new Journey();

        private const double MarkerIntersectionRangeInMetres = 200;
        private const double MarkerDisplayRangeInMetres = 1000;

        private readonly IPollutionRepository _pollutionRepo;
        private readonly ISchoolRepository _schoolRepo;
        private readonly IJourneyRepository _journeyRepo;
        private readonly IIntersectionService _interactionService;
        private readonly IDirectionService _directionService;

        public MapController(IPollutionRepository pollutionRepo, ISchoolRepository schoolRepo,
            IJourneyRepository journeyRepo, IIntersectionService interactionService,
            IDirectionService directionService)
        {
            _pollutionRepo = pollutionRepo;
            _schoolRepo = schoolRepo;
            _journeyRepo = journeyRepo;
            _interactionService = interactionService;
            _directionService = directionService;
        }

        // GET api/map
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            return await this.Get(1, true, true, new TimeSpan(9, 0, 0), "North Greenwich", 0.00447m, 51.49847m, "Westminster", -0.13563m, 51.4975m, (new Random().Next()));
        }

        [Route("routes/{journeyId:int}/{showPollution:bool}/{showSchools:bool}/{startTime:DateTime}")]
        public async Task<ActionResult<List<RouteInfo>>> RouteInfo(int journeyId, bool showPollution, bool showSchools, DateTime startTime)
        {
            RouteOptions fullJourneyOptions = await this.ProcessJourney(journeyId, new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second), showPollution, showSchools);

            return CreateRouteInfo(fullJourneyOptions, showPollution, showSchools);
        }

        [Route("pollution")]
        [HttpGet]
        public async Task<ActionResult<List<HeatMapPoint>>> HeatMap()
        {
            var markers = this._pollutionRepo.GetMarkers();
            var t = markers.Select(x => new HeatMapPoint()
            {
                Location = x.Coordinate,
                Weight = x.Value * 10000
            }).ToList();

            return t;
        }

        private async Task<Journey> GetJourney(int journeyId, double startLongitude, double startLatitude, double endLongitude, double endLatitude)
        {
            journey = _journeyRepo.GetJourney(journeyId);

            //var firstGeoCoordinate = new GeoCoordinate(startLatitude, startLongitude);
            //var secondGeoCoordinate = new GeoCoordinate(journey.Start.Latitude, journey.End.Longitude);

            //if (firstGeoCoordinate.GetDistanceTo(secondGeoCoordinate) > 200)
            //{
            //    //  Start Lng = 0.00447,   Start Lat = 51.49847
            //    //  End Lng = -0.13563,     End Lat = 51.4975
            //    var xmlResponse = await _directionService.GetAsync(new Coordinate(startLongitude, 51.49847),
            //                            new Coordinate(endLongitude, endLatitude));

            //    journey = ParseResponseToPopulateRouteOption(xmlResponse, startLongitude, startLatitude, endLongitude, endLatitude);
            //}

            return journey;
        }

        private Journey ParseResponseToPopulateRouteOption(string response, double startLongitude, double startLatitude, double endLongitude, double endLatitude)
        {
            var txtXml = response.Replace("&nbsp;", "&#160;");

            TextReader xmlReader = new StringReader(txtXml);

            var xml = XDocument.Load(xmlReader);

            var rt = from route in xml.Root.Descendants("route")
                     let routesummary = route.Element("summary").Value
                     let leg = route.Element("leg")
                     let legduration = leg.Element("duration")
                     let legdistance = leg.Element("distance")
                     let leglocation = leg.Element("start_location")
                     let steps = leg.Elements("step").Elements("start_location").ToArray()
                     select new
                     {
                         summary = routesummary,
                         duration = legduration.Element("value").Value,
                         legLocationLat = leglocation.Element("lat").Value,
                         legLocationLng = leglocation.Element("lng").Value,
                         distance = legdistance.Element("value").Value,
                         stepLocatoins = from l in steps
                                         let lat = l.Element("lat").Value
                                         let lng = l.Element("lng").Value
                                         select new Coordinate
                                         {
                                             Longitude = double.Parse(lng),
                                             Latitude = double.Parse(lat)
                                         }
                     };

            journey.Routes = new List<Route>();
            journey.JourneyId = 1;
            journey.Start = new PointDetails("Start", startLongitude, startLatitude);
            journey.End = new PointDetails("End", endLongitude, endLatitude);

            try
            {
                foreach (var r in rt)
                {
                    var route = new Route(r.stepLocatoins.ToList())
                    {
                        Distance = decimal.Parse(r.distance),
                        Duration = r.duration,
                    };

                    journey.Routes.Add(route);
                }
            }
            catch (Exception exp)
            {
                var x = exp.ToString();
            }

            return journey;

        }

        private ActionResult<List<RouteInfo>> CreateRouteInfo(RouteOptions fullJourneyOptions, bool showPollution, bool showSchools)
        {
            if (fullJourneyOptions == null)
            {
                return BadRequest();
            }

            List<EnrichedRoute> enrichedRoutes = fullJourneyOptions.EnrichedRoute.ToList();

            List<RouteInfo> routeInfos = new List<RouteInfo>();

            foreach (var enrichedRoute in enrichedRoutes)
            {
                routeInfos.Add(
                    new RouteInfo
                    {
                        ColorInHex = $"#{enrichedRoute.Colour.Substring(6, 2)}{enrichedRoute.Colour.Substring(4, 2)}{enrichedRoute.Colour.Substring(2, 2)}",
                        PollutionPoint = enrichedRoute.GreenScore,
                        PollutionZone = showPollution ? enrichedRoute.PollutionMarkers.Average(p => (decimal)p.Value) : (decimal?)null,
                        RouteLabel = enrichedRoute.Label,
                        SchoolCount = showSchools ? enrichedRoute.SchoolMarkers?.Count ?? 0 : (int?)null,
                        TravelCost = enrichedRoute.Cost,
                        Duration = CalculateTime(enrichedRoute),
                        Distance = enrichedRoute.Distance,
                        ModeOfTransport = enrichedRoute.ModeOfTransport
                    });
            }

            return Ok(routeInfos);
        }

        private static string CalculateTime(EnrichedRoute enrichedRoute)
        {
            var duration = string.Empty;

            if (!string.IsNullOrWhiteSpace(enrichedRoute.Duration))
            {
                int min = int.Parse(enrichedRoute.Duration);

                duration = $"{min} m";
                if (min > 59)
                {
                    int hr = min / 60;
                    int mod = min % 60;

                    duration = mod > 0 ? $"{hr} h {mod} m" : $"{hr} h";
                }
            }

            return duration;
        }

        // GET api/map/1?showPollution=true&showSchools=true&startTime=09:00:00&startName=NorthGreenwich&startLongitude=0.00447&startLatitude=51.49847&endName=Westerminster&endLongitude=-0.13563&endLatitude=51.4975&rand=12
        [HttpGet]
        [Route("{journeyId}")]
        public async Task<ActionResult<string>> Get(int journeyId, [FromQuery]bool showPollution, [FromQuery]bool showSchools, [FromQuery]TimeSpan startTime, [FromQuery]string startName, [FromQuery]decimal startLongitude, [FromQuery]decimal startLatitude, [FromQuery]string endName, [FromQuery]decimal endLongitude, [FromQuery]decimal endLatitude, [FromQuery]decimal rand)
        {
            RouteOptions fullJourneyOptions = await this.ProcessJourney(journeyId, startTime, showPollution, showSchools);

            var kmlString = await this.CreateTestKmlString(fullJourneyOptions, journeyId, showPollution, showSchools);
            return kmlString;
        }

        [HttpGet()]
        [Route("mobile")]
        public async Task<ActionResult<Map>> GetForMobile()
        {
            return await GetForMobile(1, true, true, new TimeSpan(9, 0, 0));
        }

        [HttpGet("mobile/{journeyId}")]
        public async Task<ActionResult<Map>> GetForMobile(int journeyId, [FromQuery]bool showPollution, [FromQuery]bool showSchools, [FromQuery]TimeSpan startTime)
        {
            var fullJourneyOptions = await this.ProcessJourney(journeyId, startTime, showPollution, showSchools);

            var map = new Map();
            map.Lines = fullJourneyOptions.EnrichedRoute.Select(r =>
            {
                var line = new Polyline(r.RouteMarkers.Select(m =>
                                new LatLng(m.Coordinate.Latitude, m.Coordinate.Longitude)))
                {
                    StrokeColor = "#" +
                                      r.Colour.Substring(6, 2) +
                                      r.Colour.Substring(4, 2) +
                                      r.Colour.Substring(2, 2) +
                                      r.Colour.Substring(0, 2),
                    StrokeWidth = 4
                };

                return line;
            }).ToList();


            map.Markers = new List<ViewModels.Marker>();

            map.Markers.Add(new ViewModels.Marker
            {
                Image = "start",
                Title = fullJourneyOptions.StartLocation.Name,
                Coordinates = new LatLng(fullJourneyOptions.StartLocation.Latitude, fullJourneyOptions.StartLocation.Longitude)
            });

            map.Markers.Add(new ViewModels.Marker
            {
                Image = "finish",
                Title = fullJourneyOptions.EndLocation.Name,
                Coordinates = new LatLng(fullJourneyOptions.EndLocation.Latitude, fullJourneyOptions.EndLocation.Longitude)
            });

            // Add route markers
            map.Markers.AddRange(GetRouteLabelsForMobile(fullJourneyOptions));

            if (showPollution)
            {
                foreach (var markers in await GetPollutionMarkersForJourney(journeyId))
                {
                    string pollutionImage = string.Empty;
                    if (markers.Value == 1)
                    {
                        pollutionImage = "one";
                    }

                    if (markers.Value == 2)
                    {
                        pollutionImage = "two";
                    }

                    if (markers.Value == 3)
                    {
                        pollutionImage = "three";
                    }

                    if (pollutionImage == string.Empty)
                    {
                        pollutionImage = "four";
                    }

                    map.Markers.Add(new ViewModels.Marker()
                    {
                        Image = pollutionImage,
                        Title = markers.Description,
                        Coordinates = new LatLng(markers.Coordinate.Latitude, markers.Coordinate.Longitude)
                    });
                }
            }

            if (showSchools)
            {
                foreach (var markers in await GetSchoolMarkersForJourney(journeyId))
                {
                    map.Markers.Add(new ViewModels.Marker()
                    {
                        Image = "school",
                        Title = markers.Description + " - " + markers.Value,
                        Coordinates = new LatLng(markers.Coordinate.Latitude, markers.Coordinate.Longitude)
                    });
                }

            }

            return map;
        }

        private async Task<string> CreateTestKmlString(RouteOptions routeOptions, int journeyId, bool showPollution, bool showSchools)
        {
            var kmlString = System.IO.File.ReadAllText(GetFilePath("Test.kml"));

            var styles = GetStyles(routeOptions);
            kmlString = kmlString.Replace("{Styles}", styles);

            var routes = GetRoutes(routeOptions);
            var routesLabels = GetRouteLabels(routeOptions);
            kmlString = kmlString.Replace("{Routes}", routes);
            kmlString = kmlString.Replace("{RouteLabels}", routesLabels);
            kmlString = kmlString.Replace("<ArrayOfFolder xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", string.Empty);
            kmlString = kmlString.Replace("</ArrayOfFolder>", string.Empty);

            if (showPollution)
            {
                var pollutionMarkers = await GetPollutionMarkersForJourney(journeyId);
                var pollutionPlacemarks = this.CreatePlacemarksVariable(pollutionMarkers, "#icon-airqualityindex-");
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
                var schoolMarkers = await GetSchoolMarkersForJourney(journeyId);
                var schoolPlacemarks = this.CreatePlacemarks(schoolMarkers, "#icon-school");
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
                sb.AppendLine(String.Format(styleString, $"line-{route.GreenScore}-{route.Cost}-{route.Colour}", route.Colour.ToLower()));
            }

            return sb.ToString();
        }

        private string styleString =
            "<Style id =\"{0}\">" +
            "<LineStyle>" +
            "<color>{1}</color>" +
            "<width>5</width>" +
            "</LineStyle>" +
            "<LabelStyle>" +
            "<scale>1</scale>" +
            "</LabelStyle>" +
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

        private string GetRouteLabels(RouteOptions routeOptions)
        {
            var folder = GetRouteLabelsFolders(routeOptions);
            var serializer = new XmlSerializer(typeof(List<Folder>));
            var xout = new StringWriter();

            serializer.Serialize(xout, folder);
            var xml = xout.ToString()
                .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty)
                .Replace("<Folder xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<Folder>");

            return xml;
        }

        private IEnumerable<ViewModels.Marker> GetRouteLabelsForMobile(RouteOptions fullJourneyOptions)
        {
            var routeMarkers = new List<ViewModels.Marker>();

            foreach (var route in fullJourneyOptions.EnrichedRoute)
            {
                var markerCoordinate = route.RouteMarkers[route.RouteMarkers.Count / 2].Coordinate;

                routeMarkers.Add(new ViewModels.Marker
                {
                    Title = route.Label,
                    Coordinates = new LatLng(markerCoordinate.Latitude, markerCoordinate.Longitude)
                });
            }

            return routeMarkers;
        }


        private List<Folder> GetRouteLabelsFolders(RouteOptions routeOptions)
        {
            var folders = new List<Folder>();

            foreach (var route in routeOptions.EnrichedRoute)
            {
                var markerCoordinate = route.RouteMarkers[route.RouteMarkers.Count / 2].Coordinate;
                folders.Add(
                    new Folder()
                    {
                        Name = $"From {routeOptions.StartLocation.Name} to {routeOptions.EndLocation.Name}",
                        Placemark = new List<Placemark>()
                        {
                            new Placemark()
                            {
                                Name = route.Label,
                                StyleUrl = $"#line-{route.GreenScore}-{route.Cost}-{route.Colour}",
                                Point = new MapApiCore.Models.Kml.Point()
                                {
                                    Coordinates = $"{markerCoordinate.Longitude},{markerCoordinate.Latitude},0"
                                }
                            }
                        }
                    });
            }

            return folders;
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
                                    Coordinates = string.Join("", route.RouteMarkers.ToList().Select(x => $"{x.Coordinate.Longitude},{x.Coordinate.Latitude},0{Environment.NewLine}"))
                                }
                            },
                            // start placemark
                            new Placemark()
                            {
                                Name = routeOptions.StartLocation.Name,
                                StyleUrl = "#icon-route-start",
                                Point = new MapApiCore.Models.Kml.Point()
                                {
                                    Coordinates = $"{routeOptions.StartLocation.Longitude},{routeOptions.StartLocation.Latitude},0"
                                }
                            },
                            // end placemark
                            new Placemark()
                            {
                                Name = routeOptions.EndLocation.Name,
                                StyleUrl = "#icon-route-end",
                                Point = new MapApiCore.Models.Kml.Point()
                                {
                                    Coordinates = $"{routeOptions.EndLocation.Longitude},{routeOptions.EndLocation.Latitude},0"
                                }
                            }
                        }
                    });
            }

            return folders;
        }

        private async Task<RouteOptions> ProcessJourney(int journeyId, TimeSpan startTime, bool showPollution, bool showSchools)
        {
            //var journeyOptions = _journeyRepo.GetJourney(journeyId);
            var journeyOptions = await GetJourney(journeyId, 0, 0, 0, 0);

            int i = 1;
            IList<EnrichedRoute> enrichedRoute = new List<EnrichedRoute>();
            foreach (var journeyOption in journeyOptions.Routes)
            {
                var er = new EnrichedRoute()
                {
                    Label = $"Option:{i}",
                    RouteMarkers = journeyOption.Coordinates.Select(x => new MapApiCore.Models.Marker(new Coordinate(x.Longitude, x.Latitude), 0, string.Empty)).ToList(),
                    PollutionMarkers = GetPollutionMarkersForRoute(journeyOption.Coordinates, startTime),
                    SchoolMarkers = GetSchoolMarkersForRoute(journeyOption.Coordinates, MarkerIntersectionRangeInMetres, startTime)
                };

                i++;
                er.Distance = Math.Round(journeyOption.Distance * 10 * 0.0006213712m, 2);
                er.Duration = journeyOption.Duration;
                er.ModeOfTransport = journeyOption.ModeOfTransport;

                var averageAirQualityIndex = er.PollutionMarkers.Average(p => (decimal)p.Value);
                var pollutionFactor = showPollution ? (int)Math.Ceiling(averageAirQualityIndex * 20) : 0;
                var schoolFactor = showSchools ? er.SchoolMarkers.Count * 40 : 0;
                Color col;

                if (journeyOption.ModeOfTransport == "bicycle")
                {
                    er.GreenScore = 95;
                    er.Cost = 0;
                    col = Color.DarkGreen;
                    er.Colour = "FF" + col.B.ToString("X2") + col.G.ToString("X2") + col.R.ToString("X2");
                }

                if (journeyOption.ModeOfTransport == "car")
                {
                    er.GreenScore = Math.Clamp(100 -pollutionFactor - schoolFactor, 0, 75);
                    er.Cost = Math.Round(((10 - ((decimal)er.GreenScore) / 10)) * er.Distance, 2);
                    col = GetBlendedColor(er.GreenScore);
                    er.Colour = "FF" + col.B.ToString("X2") + col.G.ToString("X2") + col.R.ToString("X2");
                }

                enrichedRoute.Add(er);
            }

            return new RouteOptions()
            {
                StartLocation = journeyOptions.Start,
                EndLocation = journeyOptions.End,
                EnrichedRoute = enrichedRoute
            };
        }

        private async Task<List<MapApiCore.Models.Marker>> GetPollutionMarkersForJourney(int journeyId)
        {
            // var journey = _journeyRepo.GetJourney(journeyId);
            var journeyOptions = await GetJourney(journeyId, 0, 0, 0, 0);

            var pollutionMarkers = this._pollutionRepo.GetMarkers();

            var matchedMarkers = new List<MapApiCore.Models.Marker>();

            foreach (var journeyRoute in journey.Routes)
            {
                var markersOnRoute = _interactionService.FindMarkersOnRoute(journeyRoute.Coordinates, pollutionMarkers, MarkerDisplayRangeInMetres);
                matchedMarkers = matchedMarkers.Union(markersOnRoute).ToList();
            }

            return matchedMarkers;
        }

        private async Task<List<MapApiCore.Models.Marker>> GetSchoolMarkersForJourney(int journeyId)
        {
            // var journey = _journeyRepo.GetJourney(journeyId);
            var journeyOptions = await GetJourney(journeyId, 0, 0, 0, 0);
            var schoolMarkers = this._schoolRepo.GetMarkers();

            var matchedMarkers = new List<MapApiCore.Models.Marker>();

            foreach (var journeyRoute in journey.Routes)
            {
                var markersOnRoute = _interactionService.FindMarkersOnRoute(journeyRoute.Coordinates, schoolMarkers, MarkerDisplayRangeInMetres);
                matchedMarkers = matchedMarkers.Union(markersOnRoute).ToList();
            }

            return matchedMarkers;
        }

        private List<MapApiCore.Models.Marker> GetPollutionMarkersForRoute(List<Coordinate> routeCoordinates, TimeSpan? startTime = null)
        {
            var pollutionMarkers = this._pollutionRepo.GetMarkers();
            var markers = _interactionService.FindMarkersOnRoute(routeCoordinates, pollutionMarkers, MarkerIntersectionRangeInMetres, startTime);

            return markers;
        }

        private List<MapApiCore.Models.Marker> GetSchoolMarkersForRoute(List<Coordinate> routeCoordinates, double rangeInMetres, TimeSpan? startTime = null)
        {
            var schoolMarkers = this._schoolRepo.GetMarkers();
            var markers = _interactionService.FindMarkersOnRoute(routeCoordinates, schoolMarkers, rangeInMetres, startTime);

            return markers;
        }

        private Color GetBlendedColor(int percentage)
        {
            if (percentage < 50)
            {
                return Interpolate(Color.Red, Color.Yellow, percentage / 50.0);
            }

            return Interpolate(Color.Yellow, Color.Green, (percentage - 50) / 50.0);
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

        private List<Placemark> CreatePlacemarks(List<MapApiCore.Models.Marker> markers, string style = "#icon-1769-0F9D58-nodesc-normal")
        {
            var placemarks = new List<Placemark>();

            foreach (var marker in markers)
            {
                placemarks.Add(new Placemark
                {
                    Name = $"{marker.Description}",
                    StyleUrl = style,
                    Point = new MapApiCore.Models.Kml.Point { Coordinates = $"{marker.Coordinate.Longitude},{marker.Coordinate.Latitude}" }
                });
            }

            return placemarks;
        }

        private List<Placemark> CreatePlacemarksVariable(List<MapApiCore.Models.Marker> markers, string stylePrefix)
        {
            var placemarks = new List<Placemark>();

            foreach (var marker in markers)
            {
                placemarks.Add(new Placemark
                {
                    Name = $"{marker.Description}",
                    StyleUrl = stylePrefix + marker.Value,
                    Point = new MapApiCore.Models.Kml.Point { Coordinates = $"{marker.Coordinate.Longitude},{marker.Coordinate.Latitude}" }
                });
            }

            return placemarks;
        }
    }
}