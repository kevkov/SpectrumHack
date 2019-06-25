import React from 'react';
import MapContainer from './components/mapContainer';
import {BrowserRouter, Route} from 'react-router-dom';
import ChargingCalculation from './components/chargingCalculation';
import BadgeContainer from './components/badgeContainer';

const App: React.FC = () => {
  return (    
      <BrowserRouter>
          <Route exact path="/" component={MapContainer} />
          <Route exact path="/ChargingCalculation" component={ChargingCalculation} />
          <Route exact path="/badgeContainer" component={BadgeContainer} />
      </BrowserRouter>    
  );
}

export default App;
