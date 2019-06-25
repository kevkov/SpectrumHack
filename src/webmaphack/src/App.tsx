import React from 'react';
import MapContainer from './components/mapContainer';
import {BrowserRouter, Route} from 'react-router-dom';
import ChargingCalculation from './components/chargingCalculation';

const App: React.FC = () => {
  return (    
    <BrowserRouter>
      <Route exact path="/" component={MapContainer} />
      <Route exact path="/ChargingCalculation" component={ChargingCalculation} />
    </BrowserRouter>    
  );
}

export default App;
