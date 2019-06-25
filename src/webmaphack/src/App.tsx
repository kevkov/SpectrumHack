import React from 'react';
import MapContainer from './components/mapContainer';

const App: React.FC = () => {
  return (    
      <BrowserRouter>
          <Route exact path="/" component={MapContainer} />
          <Route exact path="/ChargingCalculation" component={ChargingCalculation} />
          <Route exact path="/Badges" component={Badges} />
      </BrowserRouter>    
  );
}

export default App;
