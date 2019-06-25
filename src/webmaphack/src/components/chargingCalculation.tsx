import React, { useEffect, useState } from 'react';
import { api } from '../api';
import { RouteInfo } from '../domain/Types';
import SideBar from './sideBar';
import Alert from 'react-bootstrap/Alert';

// Demo styles, see 'Styles' section below for some notes on use.
import 'react-accessible-accordion/dist/fancy-example.css';

const ChargingCalculation: React.FC = () => {
  const googleApiKey = 'AIzaSyBoUQ0ymaQRt_Fxci5SI0EZvv_lDRBNdWM';
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
  }, [showPollution, showSchools, startTime]);

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
                  <h3>Charging Calculation</h3>

                </div>
                <div>

                  <h4>How we calculate your green score and cost</h4>
                </div>
                <div className="pt-3 pb-1 ">
                  <Alert variant="success" >
                    <Alert.Heading>Green Score Calculation</Alert.Heading>
                    <p></p>
                    <p>
                      Green Score = 100 - (Air Quality + School)
                  
  </p>


                  </Alert>
                  <Alert variant="success" >
                    <Alert.Heading>Cost Calculation</Alert.Heading>
                    <p></p>

                    <p>
                      Cost = (10 - (Green Score x 0.1)) x Distance
                  
  </p>

                  </Alert>
                  <Alert variant="success" >
                  <Alert.Heading>Weightings</Alert.Heading>
                    <p></p>

                    <p>
                      Air Quality = 20 per air quality index point
  </p>
                    <p>
                      Schools = 40 per school on journey
  </p>
                    <p>
                      Car green score capped at 75
  </p>

                  </Alert>

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

export default ChargingCalculation;
