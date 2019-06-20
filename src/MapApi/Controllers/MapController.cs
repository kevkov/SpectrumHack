﻿using GoogleMapAPIWeb.Models;
using MapApi.ViewModels;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MapApi.Controllers
{
    using MapApiCore.Interfaces;
    using MapApiCore.Models;
    using MapApiCore.Models.Kml;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
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
            return await this.Get(1, true, true, new TimeSpan(9, 0, 0), "North Greenwich", 0.00447m, 51.49847m, "Westminster", -0.13563m, 51.4975m);
        }

        [Route("routes/{journeyId:int}/{showPollution:bool}/{showSchools:bool}/{startTime:DateTime}")]
        public async Task<ActionResult<List<RouteInfo>>> RouteInfo(int journeyId, bool showPollution, bool showSchools, DateTime startTime)
        {
            RouteOptions fullJourneyOptions = await this.ProcessJourney(journeyId, new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second), showPollution, showSchools);

            return CreateRouteInfo(fullJourneyOptions); ;
        }

        private async Task<Journey> GetJourney()
        {
            var response = await _directionService.GetAsync(new Coordinate(0.00447, 51.49847),
                new Coordinate(-0.13563, 51.4975));

            return ParseResponseToPopulateRouteOption(response);
        }


        private Journey ParseResponseToPopulateRouteOption(string response)
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

            Journey journey = new Journey();
            journey.Routes = new List<Route>();
            try
            {
                foreach (var r in rt)
                {
                    var route = new Route(r.stepLocatoins.ToList())
                    {
                        Distance = decimal.Parse(r.distance),
                        Duration = r.duration
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

        private ActionResult<List<RouteInfo>> CreateRouteInfo(RouteOptions fullJourneyOptions)
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
                            ColorInHex =$"#{enrichedRoute.Colour.Substring(6,2)}{enrichedRoute.Colour.Substring(4, 2)}{enrichedRoute.Colour.Substring(2, 2)}" ,
                            PollutionPoint = enrichedRoute.GreenScore,
                            PollutionZone = enrichedRoute.PollutionMarkers.Count,
                            RouteLabel = enrichedRoute.Label,
                            SchoolCount = enrichedRoute.SchoolMarkers?.Count ?? 0,
                            TravelCost = enrichedRoute.Cost,
                            Duration = CalculateTime(enrichedRoute),
                            Distance = enrichedRoute.Distance ,
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

                    duration = mod > 0? $"{hr} h {mod} m": $"{hr} h";
                }
            }

             return duration;
        }

        // GET api/map/1?showPollution=true&showSchools=true&startTime=09:00:00&startName=NorthGreenwich&startLongitude=0.00447&startLatitude=51.49847&endName=Westerminster&endLongitude=-0.13563&endLatitude=51.4975
        [HttpGet]
        [Route("{journeyId}")]
        public async Task<ActionResult<string>> Get(int journeyId, [FromQuery]bool showPollution, [FromQuery]bool showSchools, [FromQuery]TimeSpan startTime, [FromQuery]string startName, [FromQuery]decimal startLongitude, [FromQuery]decimal startLatitude, [FromQuery]string endName, [FromQuery]decimal endLongitude, [FromQuery]decimal endLatitude)
        {
            RouteOptions fullJourneyOptions = await this.ProcessJourney(journeyId, startTime, showPollution, showSchools);

            var kmlString = this.CreateTestKmlString(fullJourneyOptions, showPollution, showSchools);
            return kmlString;
        }

        [HttpGet("mobile")]
        public async Task<ActionResult<Map>> GetForMobile()
        {
            var showPollution = true;
            var showSchools = true;

            RouteOptions fullJourneyOptions = await this.ProcessJourney(1, new TimeSpan(9, 0, 0), showPollution, showSchools);

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

            if (showPollution)
            {
                foreach (var markers in this._pollutionRepo.GetMarkers())
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
                foreach (var markers in this._schoolRepo.GetMarkers())
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

        private string CreateTestKmlString(RouteOptions routeOptions, bool showPollution, bool showSchools)
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
                var pollutionMarkers = this._pollutionRepo.GetMarkers();
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
                var schoolMarkers = this._schoolRepo.GetMarkers();
                var schoolPlacemarks = this.CreatePlacemarks(schoolMarkers, "#icon-school");
                var folder = new Folder { Name = "Schools", Placemark = schoolPlacemarks };
                var serializer = new XmlSerializer(typeof(Folder));
                var xout = new StringWriter();

                serializer.Serialize(xout, folder);
                var xml = xout.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", string.Empty);
                kmlString = kmlString.Replace("{Schools}", xml);
                kmlString = kmlString.Replace("{SchoolsStyle}", xmlSchoolStyle);
            }
            else
            {
                kmlString = kmlString.Replace("{Schools}", string.Empty);
                kmlString = kmlString.Replace("{SchoolsStyle}", string.Empty);
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
            "</Style>";

        private string xmlSchoolStyle =
            "<Style id=\"icon-school\">" +
            "<IconStyle>" +
            "<color>ff00ff00</color>" +
            "<scale>1</scale>" +
            "<Icon>" +
            "<href>http://maps.google.com/mapfiles/kml/pal3/icon56.png</href>" +
            "</Icon>" +
            "</IconStyle>" +
            "<LabelStyle>" +
            "<scale>0</scale>" +
            "</LabelStyle>" +
            "<BalloonStyle>" +
            "<text><![CDATA[$[name]]]></text>" +
            "</BalloonStyle>" +
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
                                Point = new Point()
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
                                Point = new Point()
                                {
                                    Coordinates = $"{routeOptions.StartLocation.Longitude},{routeOptions.StartLocation.Latitude},0"
                                }
                            },
                            // end placemark
                            new Placemark()
                            {
                                Name = routeOptions.EndLocation.Name,
                                StyleUrl = "#icon-route-end",
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

        private async Task<RouteOptions> ProcessJourney(int journeyId, TimeSpan startTime, bool showPollution, bool showSchools)
        {
            var journeyOptions = _journeyRepo.GetJourney(journeyId);
           // var journeyOptions = await GetJourney();
            var pollutionMarkers = this._pollutionRepo.GetMarkers();
            var schoolMarkers = this._schoolRepo.GetMarkers();

            int i = 0;
            IList<EnrichedRoute> enrichedRoute = new List<EnrichedRoute>();
            foreach (var journeyOption in journeyOptions.Routes)
            {
                var er = new EnrichedRoute()
                {
                    Label = $"Option:{i}",
                    RouteMarkers = journeyOption.Coordinates.Select(x => new Marker(new Coordinate(x.Longitude, x.Latitude), 0, string.Empty)).ToList(),
                    PollutionMarkers = _interactionService.FindMarkersOnRoute(journeyOption.Coordinates, pollutionMarkers, startTime),
                    SchoolMarkers = _interactionService.FindMarkersOnRoute(journeyOption.Coordinates, schoolMarkers, startTime)
                };

                i++;
                er.Distance =Math.Round(journeyOption.Distance * 10 * 0.0006213712m,2);
                er.Duration = journeyOption.Duration;
                er.ModeOfTransport = journeyOption.ModeOfTransport;

                var pollutionFactor = showPollution ? er.PollutionMarkers.Count * 10 : 0;
                var schoolFactor = showSchools ? er.SchoolMarkers.Count * 20 : 0;

                if (journeyOption.ModeOfTransport == "cycle")
                {
                    er.GreenScore = 95;
                    er.Cost = 0;
                }

                // pollution level from json of car increases factor
                // ui shows follution factor
                // how many school and pollution zones crossed
                // mode of transport

                // show factor in the UI
                // extend the ui model and add in modeoftransport, cost multiplication factor, how many schools and pollution zones crossed, car pollution rate

                // high and low polluting vrm cars

                // or in box also show if using cleaning car - cost would be 

                // or just give a message that cleaners cars cost less.

                //message if taken greenest route £2 added to oyster account for future public transport journey
                // and and 5 pts or something

                // side bar

                // badges in side bar show

                //When changing the Air Quality Index/ Time / Schools options...the map reloads and zooms out... fix this

                //6 Add some content to the side bar(Neil to provide some sample content

                if (journeyOption.ModeOfTransport == "car")
                {
                    er.GreenScore = Math.Clamp(100 - pollutionFactor - schoolFactor, 0, 75);
                    er.Cost = Math.Round(((10 - ((decimal)er.GreenScore) / 10)) * er.Distance , 2);
                }

                var col = GetBlendedColor(er.GreenScore);
                er.Colour = col.A.ToString("X2") + col.B.ToString("X2") + col.G.ToString("X2") + col.R.ToString("X2");
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
            {
                return Interpolate(Color.Red, Color.Yellow, percentage / 50.0);
            }

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

        private List<Placemark> CreatePlacemarks(List<Marker> markers, string style = "#icon-1769-0F9D58-nodesc-normal")
        {
            var placemarks = new List<Placemark>();

            foreach (var marker in markers)
            {
                placemarks.Add(new Placemark
                {
                    Name = $"{marker.Description}",
                    StyleUrl = style,
                    Point = new Point { Coordinates = $"{marker.Coordinate.Longitude},{marker.Coordinate.Latitude}" }
                });
            }

            return placemarks;
        }


        private List<Placemark> CreatePlacemarksVariable(List<Marker> markers, string stylePrefix)
        {
            var placemarks = new List<Placemark>();

            foreach (var marker in markers)
            {
                placemarks.Add(new Placemark
                {
                    Name = $"{marker.Description}",
                    StyleUrl = stylePrefix + marker.Value,
                    Point = new Point { Coordinates = $"{marker.Coordinate.Longitude},{marker.Coordinate.Latitude}" }
                });
            }

            return placemarks;
        }
    }
}