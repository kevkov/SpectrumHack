import React, { useEffect, useState} from 'react';
import { api } from '../api';
import { RouteInfo } from '../domain/Types';
import SideBar from './sideBar';
import RouteMap from './routeMap';

// Demo styles, see 'Styles' section below for some notes on use.
import 'react-accessible-accordion/dist/fancy-example.css';

const MapContainer: React.FC = () => {
    const googleApiKey = 'AIzaSyCmLOoMOOcDlU9zIoKH0_UfAsUUlCE6PRM';
    const journeyId = 1;
    const [showPollution, togglePollution] = useState(() => false);
    const [showSchools, toggleSchools] = useState(() => false);
    const [startTime, setStartTime] = useState(() => '12:00');
    const [showHeatmap, toggleHeatmap] = useState(() => false);
    const [routeInfoItems, setRouteInfoItems] = useState<RouteInfo[]>(() => []);

    useEffect(() => {
        var uri = "http://spectrummapapi.azurewebsites.net/api/map/routes/1/" +        
        showPollution + "/" +
        showSchools + "/" +
        startTime;

        console.log('Calling api at: ' + uri);
        
        api<RouteInfo[]>(uri)
            .then(data => {
                console.log(`api callback in journey details ${uri}`);
                setRouteInfoItems(data);
            });
    },[showPollution, showSchools, startTime]);

  return (
    <div>    
    <header>
      <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark border-bottom box-shadow mb-3">
        <div className="container-fluid">
            <a className="navbar-brand text-white" href="/">Spectrum Hackathon</a>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            <div className="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                <ul className="navbar-nav flex-grow-1">
                    <li className="nav-item">
                        <a className="nav-link text-light" href="/">Home</a>
                    </li>
                </ul>
            </div>
            <form className="form-inline">
                <span className="navbar-text text-white pl-2 pr-2">Start Time:</span>
                <select id="startTimeList" className="form-control mr-sm-2" value={startTime} onChange={(event) => setStartTime(event.target.value)}>
                    <option>00:00</option>
                    <option>01:00</option>
                    <option>02:00</option>
                    <option>03:00</option>
                    <option>04:00</option>
                    <option>05:00</option>
                    <option>06:00</option>
                    <option>07:00</option>
                    <option>08:00</option>
                    <option>09:00</option>
                    <option>10:00</option>
                    <option>11:00</option>
                    <option>12:00</option>
                    <option>13:00</option>
                    <option>14:00</option>
                    <option>15:00</option>
                    <option>16:00</option>
                    <option>17:00</option>
                    <option>18:00</option>
                    <option>19:00</option>
                    <option>20:00</option>
                    <option>21:00</option>
                    <option>22:00</option>
                    <option>23:00</option>
                </select>
                <span className="navbar-text text-white pl-2 pr-2">From:</span>
                <input className="form-control mr-sm-2" type="text" placeholder="From" aria-label="From" value="North Greenwich" readOnly />
                <span className="navbar-text text-white pl-2 pr-2">To:</span>
                <input className="form-control mr-sm-2" type="text" placeholder="To" aria-label="To" value="Westminster" readOnly />
                <button className="btn btn-success my-2 my-sm-0" type="submit">Show Route</button>
            </form>
        </div>
        </nav>
    </header>
    <div>
        <main role="main" className="pb-3">
        <div className="container-fluid">    
            <div className="row">
                <SideBar />
                <main role="main" className="col-md-9 ml-sm-auto col-lg-10 px-4">

                    <div className="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-1 pb-1 mb-3 border-bottom">
                        <h3>Route Map</h3>
                        <div className="btn-toolbar mb-2 mb-md-0">
                            <div className="btn-group mr-2">
                                <button type="button" id="btnSchools" className={"btn btn-sm " + (showSchools ? 'btn-primary' : 'btn-outline-primary')} onClick={() => toggleSchools(!showSchools)}>Schools</button>
                                <button type="button" id="btnHeatmap" className={"btn btn-sm " + (showHeatmap ? 'btn-primary' : 'btn-outline-primary')} onClick={() => toggleHeatmap(!showHeatmap)}>Air Quality Heatmap</button>
                                <button type="button" id="btnPollution" className={"btn btn-sm " + (showPollution ? 'btn-primary' : 'btn-outline-primary')} onClick={() => togglePollution(!showPollution)}>Air Quality Index</button>
                            </div>
                        </div>
                    </div>
                    
                    <RouteMap
                        googleMapURL={`https://maps.googleapis.com/maps/api/js?key=${googleApiKey}&v=3.exp&libraries=geometry,drawing,places,visualization,drawing,places`}
                        loadingElement={<div style={{ height: `100%` }} />}
                        containerElement={<div style={{ height: `400px` }} />}
                        mapElement={<div style={{ height: `100%` }} />}
                        showPollution={showPollution}
                        showSchools={showSchools}
                        startTime={startTime}
                        showHeatmap={showHeatmap}
                              />

                    <div className="row pt-3">
                             
                        <div className="container">
                            <div className="card-deck mb-3 text">
                                
                                {routeInfoItems.map((item) => {
                                    return <div key={item.routeLabel} className="card border-secondary mb-4 shadow-sm">
                                        <div className="card-header" style={{backgroundColor: item.colorInHex}}>
                                            <h4> {item.routeLabel} ({item.modeOfTransport}) </h4>
                                        </div>
                                        <div className="card-body bg-light nopadding">
                                            <ul className="list-unstyled mt-2 pl-2">
                                                <li><h6>Green score: {item.pollutionPoint}</h6></li>
                                                <li><h6>Schools: {item.schoolCount === null || item.schoolCount === undefined ? "N/A" : item.schoolCount}</h6></li>
                                                <li><h6>Distance: {item.distance} miles</h6></li>
                                                <li><h6>Average Air Quality: {item.pollutionZone === null || item.pollutionZone === undefined ? "N/A" : item.pollutionZone.toFixed(2)}</h6></li>
                                                <li><h6>Travel time: {item.duration}</h6></li>
                                                <li><h6>Travel cost: £{item.travelCost.toFixed(2)}</h6></li>
                                                <div>
                                                  
                                                </div>
                                            </ul>
                                        </div>
                                    </div>
                                    })
                                }                    
                            </div>
                        </div>
                    </div>
                </main>
            </div>
        </div>
        </main>
    </div>

    <footer className="border-top footer text-muted">
        <div className="container">
            &copy; 2019 - Spectrum
        </div>
    </footer>
  </div>    
  );
}

export default MapContainer;
